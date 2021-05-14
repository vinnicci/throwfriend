using Godot;
using System;

public abstract class EnemyProj : Area2D
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


    public void Spawn(Vector2 globalPos, float globalRotDeg, Level lvl) {
        Range = range;
        Speed = speed;
        currentRange = Range;
        Velocity = new Vector2(Speed, 0).Rotated(Godot.Mathf.Deg2Rad(globalRotDeg));
        GlobalPosition = globalPos;
        GlobalRotation = globalRotDeg;
        lvl.AddChild(this);
    }


    private void OnEnemyProjBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
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
