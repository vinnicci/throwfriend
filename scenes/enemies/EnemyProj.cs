using Godot;
using System;

public abstract class EnemyProj : Area2D, ISpawnable
{
    [Export] protected int range = 500;
    [Export] protected int speed = 20;
    [Export] protected int damage = 1;
    [Export] protected int homing_magnitude = 0;

    Godot.Collections.Array hitExceptions = new Godot.Collections.Array();
    public Vector2 Velocity {get; private set;}
    public int Damage {get; set;}
    public int Range {get; set;}
    public int Speed {get; set;}    
    public Level LevelNode {get; set;}

    protected Sprite sprite;
    Explosion explosionNode;
    AnimationPlayer anim;
    protected AudioStreamPlayer2D sound;
    const int KNOCKBACK = 250;


    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("Sprite");
        anim = (AnimationPlayer)GetNode("Anim");
        sound = (AudioStreamPlayer2D)GetNode("Sound");
    }


    int currentRange;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(currentRange <= 0) {
            if(Velocity != Vector2.Zero) {
                StopProjectile();
            }
            return;
        }
        if(Velocity != Vector2.Zero) {
            sprite.GlobalRotation = Velocity.Angle();
        }
        if(homing_magnitude > 0) {
            SeekPlayer();
        }
        currentRange -= Speed;
        Position += Velocity;
    }


    void SeekPlayer() {
        Vector2 v = GlobalPosition + Velocity;
        Vector2 home_target = (LevelNode.GetPlayerPos() - v).Normalized() * homing_magnitude;
        Velocity += home_target;
        Velocity = Velocity.Clamped(Speed);
    }


    public void AddHitException(Entity body) {
        hitExceptions.Add(body);
    }


    public virtual void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRotDeg = 0) {
        if(destination != Vector2.Zero) {
            Range = CalculateRange(destination);
        }
        else {
            Range = range;
        }
        Speed = speed;
        Damage = (int)(damage * lvl.enemyHealthMult);
        LevelNode = lvl;
        if(HasNode("Explosion")) {
            explosionNode = (Explosion)GetNode("Explosion");
            explosionNode.Damage = Damage;
            explosionNode.LevelNode = LevelNode;
        }
        currentRange = Range;
        Velocity = new Vector2(Speed, 0).Rotated(Mathf.Deg2Rad(globalRotDeg));
        lvl.Spawn(this, globalPos, Mathf.Deg2Rad(globalRotDeg));
    }


    const int MIN_RANGE = 150; 


    int CalculateRange(Vector2 dest) {
        int projRange = (int)dest.Length() - 50;
        if(projRange <= MIN_RANGE) {
            projRange = MIN_RANGE + 50;
        }
        return projRange;
    }


    public virtual bool OnEnemyProjBodyEntered(Godot.Object body) {
        if(hitExceptions.Contains(body)) {
            return false;
        }
        bool result = false;
        if(body is IHealthModifiable && ((IHealthModifiable)body).HitCooldown.IsStopped()) {
            ((IHealthModifiable)body).Hit((((Node2D)body).GlobalPosition - GlobalPosition).Clamped(1) *
            KNOCKBACK, Damage);
            result = true;
        }
        StopProjectile();
        return result;
    }


    public virtual void StopProjectile() {
        sprite.GlobalRotation = Velocity.Angle();
        Velocity = Vector2.Zero;
        if(anim.IsPlaying() && anim.CurrentAnimation != "hit") {
            anim.Stop();
            anim.Play("hit");
        }
        else {
            anim.Play("hit");
        }
        if(IsInstanceValid(sound)) {
            sound.GetParent().RemoveChild(sound);
            LeaveObj(sound, 2f);
            sound.Play();
        }
        if(IsInstanceValid(explosionNode)) {
            explosionNode.Explode();
            sprite.Visible = false;
        }
    }


    public virtual void ReturnToPool() {
        QueueFree();
    }


    protected void LeaveObj(Node2D obj, float duration) {
        LevelNode.AddChild(obj);
        Tween tween = new Tween();
        obj.AddChild(tween);
        obj.GlobalPosition = GlobalPosition;
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
