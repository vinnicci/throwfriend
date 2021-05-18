using Godot;
using System;

public abstract class EnemyProj : Area2D, ISpawnable
{
    [Export] protected int range = 500;
    [Export] protected int speed = 20;
    [Export] protected int damage = 1;

    private Godot.Collections.Array hitExceptions = new Godot.Collections.Array();
    public Vector2 Velocity {get; private set;}
    public int Damage {get; set;}
    public int Range {get; set;}
    public int Speed {get; set;}

    private Sprite sprite;
    private Explosion explosion;
    private AnimationPlayer anim;
    const int KNOCKBACK = 300;


    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("Sprite");
        anim = (AnimationPlayer)GetNode("Anim");
        if(HasNode("Explosion") == true) {
            explosion = (Explosion)GetNode("Explosion");
        }
    }


    private float currentRange;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(currentRange <= 0) {
            StopProjectile();
            return;
        }
        if(Velocity != Vector2.Zero) {
            sprite.GlobalRotation = Velocity.Angle();
        }
        currentRange -= Speed;
        Position += Velocity;
    }


    public void AddHitException(Entity body) {
        hitExceptions.Add(body);
    }


    public void Spawn(Level lvl, Vector2 globalPos, float globalRotDeg = 0) {
        Range = range;
        Speed = speed;
        Damage = damage;
        if(IsInstanceValid(explosion) == true) {
            explosion.Damage = Damage;
        }
        currentRange = Range;
        Velocity = new Vector2(Speed, 0).Rotated(Godot.Mathf.Deg2Rad(globalRotDeg));
        lvl.Spawn(this, globalPos, Godot.Mathf.Deg2Rad(globalRotDeg));
    }


    private void OnEnemyProjBodyEntered(Godot.Object body) {
        if(hitExceptions.Contains(body) == true) {
            return;
        }
        if(body is IHealthModifiable && ((IHealthModifiable)body).HitCooldown.IsStopped() == true) {
            ((IHealthModifiable)body).Hit((((Node2D)body).GlobalPosition - GlobalPosition).Clamped(1) *
            KNOCKBACK, Damage);
            StopProjectile();
        }
    }


    private void StopProjectile() {
        if(Velocity != Vector2.Zero) {
            sprite.GlobalRotation = Velocity.Angle();
            Velocity = Vector2.Zero;
            anim.Play("hit");
            if(IsInstanceValid(explosion) == true) {
                explosion.Explode();
            }
        }
    }


}
