using Godot;
using System;

public class Entity : RigidBody2D
{
    [Export] protected int speed = 500;
    public int Speed {get; set;}
    [Export] protected int health = 1;
    public int Health {get; set;}
    public Vector2 Velocity {get; protected set;}
    protected Timer deathTimer;


    public override void _Ready()
    {
        deathTimer = (Timer)GetNode("DeathTimer");
        Speed = speed;
        Health = health;
    } 


    public override void _PhysicsProcess(float delta)
    {
        if(ContinuousCd == RigidBody2D.CCDMode.Disabled && LinearVelocity.LengthSquared() > Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.CastRay;
        }
        else if(ContinuousCd == RigidBody2D.CCDMode.CastRay && LinearVelocity.LengthSquared() <= Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.Disabled;
        }
    }


    private Vector2 teleportPos;


    public void Teleport(Vector2 global_pos) {
        teleportPos = global_pos;
    }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        //teleport
        if(teleportPos != Vector2.Zero) {
            Transform2D trans = new Transform2D(Rotation, teleportPos);
            state.Transform = trans;
		    teleportPos = Vector2.Zero;
        }
        //velocity
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity.Clamped(1) * Speed);
        Velocity = Vector2.Zero;
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
