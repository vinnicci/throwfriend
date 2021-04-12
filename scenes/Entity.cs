using Godot;
using System;

public class Entity : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public int speed = 1000;

    public Vector2 Velocity {get; protected set;}
    protected Vector2 knockback;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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
        state.AddCentralForce(Velocity.Clamped(1) * speed);
    }


}
