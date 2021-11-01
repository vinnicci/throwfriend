using Godot;
using System;

public abstract class Entity : RigidBody2D, IHealthModifiable, ITeleportable, ISpawnable, ISoundEmitter
{
    [Export] public int speed = 250;
    public int Speed {get; set;}
    [Export] public int health = 1;
    public int Health {get; set;}
    
    public Vector2 Velocity {get; set;}
    public Timer HitCooldown {get; set;}
    public AnimationPlayer TeleportAnim {get; set;}
    public AnimationPlayer DamageAnim {get; set;}
    public Node2D SoundsNode {get; set;}

    protected Level levelNode;
    public virtual Level LevelNode {
        get {
            return levelNode;
        }
        set{
            levelNode = value;
        }
    }
    protected AnimationPlayer anim;
    protected Node2D spriteNode;
    protected Node2D hud;
    HealthHUD healthHUD;
    

    public override void _Ready()
    {
        base._Ready();
        Health = health;
        Speed = speed;
        hud = (Node2D)GetNode("HUD");
        spriteNode = (Node2D)GetNode("Sprite");
        TeleportAnim = (AnimationPlayer)GetNode("Anims/TeleAnim");
        DamageAnim = (AnimationPlayer)GetNode("Anims/DamageAnim");
        anim = (AnimationPlayer)GetNode("Anims/Anim");
        HitCooldown = (Timer)GetNode("HitCooldown");
        healthHUD = (HealthHUD)hud.GetNode("Health");
        SoundsNode = (Node2D)GetNode("Sounds");
        SoundsDict = new Godot.Collections.Dictionary<String, AudioStreamPlayer2D>();
        healthHUD.ParentNode = this;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        hud.GlobalRotation = 0;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        int lv = (int)LinearVelocity.LengthSquared();
        if(ContinuousCd == RigidBody2D.CCDMode.Disabled && lv > Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.CastRay;
        }
        else if(ContinuousCd == RigidBody2D.CCDMode.CastRay && lv <= Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.Disabled;
        }
    }


    Vector2 teleportPos;


    public void Teleport(Level level, Vector2 global_pos) {
        //sprite fade out effect
        Node2D teleSprite = (Node2D)GetNode("Sprite").Duplicate();
        level.AddChild(teleSprite);
        Tween tween = new Tween();
        teleSprite.AddChild(tween);
        teleSprite.GlobalPosition = GlobalPosition;
        teleSprite.GlobalRotation = GlobalRotation;
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(teleSprite);
        if(tween.IsConnected("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject)) == false) {
            tween.Connect("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject), arr);
        }
        tween.InterpolateProperty(teleSprite, "scale", teleSprite.Scale, new Vector2(0.01f, teleSprite.Scale.y), 0.25f,
        Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
        //teleport
        teleportPos = global_pos;
        TeleportAnim.Stop();
        TeleportAnim.Play("teleported");
        PlaySoundEffect("Teleport");
    }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        if(Health <= 0) {
            AppliedForce = Vector2.Zero;
            return;
        }
        //teleport
        if(teleportPos != Vector2.Zero) {
            Transform2D trans = new Transform2D(Rotation, teleportPos);
            state.Transform = trans;
		    teleportPos = Vector2.Zero;
        }
        //velocity
        AdjustSprites();
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity * Speed);
        Velocity = Vector2.Zero;
    }


    public virtual void AdjustSprites() {
        //running
        if(anim.CurrentAnimation != "run" && anim.CurrentAnimation != "idle") {
            return;
        }
        if(Velocity == Vector2.Zero) {
            anim.Play("idle");
        }
        else {
            anim.Play("run");
        }
        if((Velocity.x < 0 && spriteNode.Scale.x > 0) || (Velocity.x > 0 && spriteNode.Scale.x < 0)) {
            Vector2 scale = new Vector2(-1,1);
            spriteNode.Scale *= scale;
        }
    }


    [Export] public float knockbackMult = 1f;
    [Signal] public delegate void Died();


    public virtual bool Hit(Vector2 knockback, int damage) {
        if(Health <= 0 || (HitCooldown.IsStopped() == false && (damage > 0 && Health > 0))) {
            return false;
        }
        ApplyCentralImpulse(knockback * knockbackMult);
        ContinuousCd = CCDMode.CastRay;
        if(damage > 0) {
            DamageAnim.Play("damaged");
            PlaySoundEffect("Hit");
        }
        Health -= damage;
        if(Health <= 0) {
            Die();
        }
        Health = Mathf.Clamp(Health, 0, health);
        healthHUD.UpdateHealth();
        return true;
    }


    public virtual void Die() {
        EmitSignal(nameof(Died));
        Health = 0;
        healthHUD.Visible = false;
        anim.Stop();
        anim.Play("die");
        //change sound node parent to level node
        SoundsNode.GetParent().RemoveChild(SoundsNode);
        LeaveObj(SoundsNode, spriteNode.GlobalPosition, 1f);
        PlaySoundEffect("Died");
        SetCollisionLayerBit(Global.BIT_MASK_ENEMY, false);
        SetCollisionLayerBit(Global.BIT_MASK_PLAYER, false);
        SetProcess(false);
        SetPhysicsProcess(false);
    }


    public async void SlowEffect(float timeScale, float duration) {
        Engine.TimeScale = timeScale;
        await ToSignal(GetTree().CreateTimer(duration), "timeout");
        Engine.TimeScale = 1;
    }


    ///<summary>Enter -1 as argument to ignore stat change</summary>
    public void ChangeEntityBaseStats(int newHealth = -1, int newSpeed = -1) {
        if(newHealth != -1) {
            health = newHealth;
            if(health <= 0) {
                health = 1;
            }
            Health = health;
            if(IsInstanceValid(healthHUD)) {
                healthHUD.ParentNode = this;
            }
        }
        if(newSpeed != -1) {
            speed = newSpeed;
            Speed = speed;
        }
    }
    

    public virtual void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0) {
        lvl.Spawn(this, globalPos, globalRot);
    }


    public Godot.Collections.Dictionary<String, AudioStreamPlayer2D> SoundsDict {get; set;}


    public void PlaySoundEffect(string soundName) {
        if(SoundsNode.HasNode(soundName) == false) {
            return;
        }
        if(SoundsDict.ContainsKey(soundName) == false) {
            SoundsDict.Add(soundName, (AudioStreamPlayer2D)SoundsNode.GetNode(soundName));
        }
        AudioStreamPlayer2D soundNode = SoundsDict[soundName];
        soundNode.Play();
    }


    //0 frame counter
    //1 frames count
    int trailFrames;


    public void LeaveTrail(int frames) {
        if(trailFrames == 0) {
            trailFrames = frames;
        }
        else if(trailFrames > 0) {
            trailFrames -= 1;
            return;
        }
        Node2D trailSprite = (Node2D)spriteNode.Duplicate();
        LeaveObj(trailSprite, spriteNode.GlobalPosition, 0.25f);
    }


    void LeaveObj(Node2D obj, Vector2 gPos, float duration) {
        LevelNode.AddChild(obj);
        Tween tween = new Tween();
        obj.AddChild(tween);
        obj.GlobalPosition = gPos;
        obj.GlobalRotation = GlobalRotation;
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(obj);
        if(tween.IsConnected("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject)) == false) {
            tween.Connect("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject), arr);
        }
        tween.InterpolateProperty(obj, "modulate", obj.Modulate, new Color(1,1,1,0), duration,
        Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }


}
