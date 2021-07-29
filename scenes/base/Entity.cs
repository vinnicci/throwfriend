using Godot;
using System;

public abstract class Entity : RigidBody2D, IHealthModifiable, ITeleportable, ISpawnable
{
    [Export] public int speed = 250;
    public int Speed {get; set;}
    [Export] public int health = 1;
    public int Health {get; set;}
    
    public Vector2 Velocity {get; set;}
    public bool IsDead {get; set;}
    public Timer HitCooldown {get; set;}
    public AnimationPlayer TeleportAnim {get; set;}
    public AnimationPlayer DamageAnim {get; set;}

    protected AnimationPlayer anim;
    protected Node2D spriteNode;
    protected Node2D hud;
    HealthHUD healthHUD;


    public override void _Notification(int what)
    {
        base._Notification(what);
        if(what == NotificationInstanced) {
            Speed = speed;
            Health = health;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        hud = (Node2D)GetNode("HUD");
        healthHUD = (HealthHUD)hud.GetNode("Health");
        healthHUD.ParentNode = this;
        spriteNode = (Node2D)GetNode("Sprite");
        TeleportAnim = (AnimationPlayer)GetNode("Anims/TeleAnim");
        DamageAnim = (AnimationPlayer)GetNode("Anims/DamageAnim");
        anim = (AnimationPlayer)GetNode("Anims/Anim");
        HitCooldown = (Timer)GetNode("HitCooldown");
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
        if(tween.IsConnected("tween_all_completed", this, nameof(FreeSprite)) == false) {
            tween.Connect("tween_all_completed", this, nameof(FreeSprite), arr);
        }
        tween.InterpolateProperty(teleSprite, "scale", teleSprite.Scale, new Vector2(0.01f, teleSprite.Scale.y), 0.25f,
        Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
        //teleport
        teleportPos = global_pos;
        TeleportAnim.Stop();
        TeleportAnim.Play("teleported");
    }


    public void FreeSprite(Godot.Object teleSprite) {
        ((Node2D)teleSprite).QueueFree();
    }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        if(IsDead == true) {
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


    [Export] float knockbackMult = 1f;
    [Signal] public delegate void Died();


    public virtual bool Hit(Vector2 knockback, int damage) {
        if(IsDead == true || (HitCooldown.IsStopped() == false && damage > 0)) {
            return false;
        }
        ApplyCentralImpulse(knockback * knockbackMult);
        if(damage > 0) {
            DamageAnim.Play("damaged");
        }
        Health -= damage;
        if(Health <= 0) {
            EmitSignal(nameof(Died));
            IsDead = true;
            anim.Stop();
            anim.Play("die");
            SetCollisionLayerBit(Global.BIT_MASK_ENEMY, false);
            SetCollisionLayerBit(Global.BIT_MASK_PLAYER, false);
        }
        Health = Godot.Mathf.Clamp(Health, 0, health);
        healthHUD.UpdateHealth();
        return true;
    }


    ///<summary>Enter -1 as argument to ignore stat change</summary>
    public void ChangeEntityBaseStats(int newHealth = -1, int newSpeed = -1) {
        if(newHealth != -1) {
            health = newHealth;
            Health = health;
            healthHUD.ParentNode = this;
        }
        if(newSpeed != -1) {
            speed = newSpeed;
            Speed = speed;
        }
    }
    

    public void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0) {
        lvl.Spawn(this, globalPos, globalRot);
    }


}
