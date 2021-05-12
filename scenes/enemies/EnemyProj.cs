using Godot;
using System;

public abstract class EnemyProj : Area2D
{
    [Export] protected int range = 500;
    [Export] protected int speed = 20;

    public Vector2 Velocity {get; private set;}
    public int Range {get; private set;}
    public int Speed {get; private set;}

    private Sprite sprite;
    private Timer rangeTimer;
    private AnimationPlayer anim;
    const int KNOCKBACK = 300;


    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("Sprite");
        rangeTimer = (Timer)GetNode("RangeTimer");
        anim = (AnimationPlayer)GetNode("Anim");
        Range = range;
        Speed = speed;
        rangeTimer.WaitTime = (float)Range/(float)Speed;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Position += Velocity;
        if(Velocity != Vector2.Zero) {
            sprite.GlobalRotation = Velocity.Angle();
        }
    }


    public void Spawn(Vector2 globalPos, float globalRotDeg, Level lvl) {
        Velocity = new Vector2(speed, 0);
        Velocity = Velocity.Rotated(Godot.Mathf.Deg2Rad(globalRotDeg));
        GlobalPosition = globalPos;
        GlobalRotation = globalRotDeg;
        lvl.AddChild(this);
        rangeTimer.Start();
    }


    private void OnEnemyProjBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
        Velocity = Vector2.Zero;
        anim.Play("hit");
        rangeTimer.Stop();
    }


    private void OnRangeTimerTimeout() {
        Velocity = Vector2.Zero;
        anim.Play("hit");
    }


}
