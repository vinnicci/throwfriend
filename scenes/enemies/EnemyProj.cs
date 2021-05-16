using Godot;
using System;

public abstract class EnemyProj : Area2D, ISpawnable
{
    [Export] public int range = 500;
    [Export] public int speed = 20;

    public Vector2 Velocity {get; private set;}
    public int Range {get; set;}
    public int Speed {get; set;}

    private Sprite sprite;
    private AnimationPlayer anim;
    const int KNOCKBACK = 300;


    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("Sprite");
        anim = (AnimationPlayer)GetNode("Anim");
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


    public void Spawn(Level lvl, Vector2 globalPos, float globalRotDeg = 0) {
        Range = range;
        Speed = speed;
        currentRange = Range;
        Velocity = new Vector2(Speed, 0).Rotated(Godot.Mathf.Deg2Rad(globalRotDeg));
        lvl.Spawn(this, globalPos, Godot.Mathf.Deg2Rad(globalRotDeg));
    }


    private void OnEnemyProjBodyEntered(Godot.Object body) {
        IHealthModifiable hitBody = (IHealthModifiable)body;
        hitBody.Hit((((Node2D)body).GlobalPosition - GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        StopProjectile();
    }


    private void StopProjectile() {
        if(Velocity != Vector2.Zero) {
            sprite.GlobalRotation = Velocity.Angle();
            Velocity = Vector2.Zero;
            anim.Play("hit");
        }
    }


}
