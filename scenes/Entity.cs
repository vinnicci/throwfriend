using Godot;
using System;

public class Entity : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] protected int speed = 500;
    [Export] protected int health = 1;

    public int Speed {get; protected set;}
    public int Health {get; protected set;}
    public Vector2 Velocity {get; protected set;}
    protected Timer deathTimer;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        deathTimer = (Timer)GetNode("DeathTimer");
        Speed = speed;
        Health = health;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //  }


    // public override void _PhysicsProcess(float delta)
    // {
    // }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity.Clamped(1) * Speed);
    }


    public virtual void Hit(Vector2 linearV, int damage) {
        ApplyCentralImpulse(linearV);
        Health -= damage;
        if(Health <= 0) {
            deathTimer.Start();
        }
    }


    public virtual void OnDeathTimerTimeout() {
        QueueFree();
    }


}
