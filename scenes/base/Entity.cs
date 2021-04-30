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
    protected Timer hitCooldown;
    protected AnimationPlayer anim;
    public bool IsDead {get; set;}

    protected AnimatedSprite legs;
    protected AnimatedSprite sprite;


    public override void _Ready()
    {
        base._Ready();
        deathTimer = (Timer)GetNode("DeathTimer");
        hitCooldown = (Timer)GetNode("HitCooldown");
        anim = (AnimationPlayer)GetNode("Anim");
        legs = (AnimatedSprite)GetNode("Sprite/Legs");
        sprite = (AnimatedSprite)GetNode("Sprite");
        Speed = speed;
        Health = health;
        IsDead = false;
    } 


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
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
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity.Clamped(1) * Speed);
        AnimateSprites();
        Velocity = Vector2.Zero;
    }


    private void AnimateSprites() {
        //running
        if(Velocity == Vector2.Zero) {
            legs.Play("idle");
        }
        else {
            legs.Play("run");
        }
        //left
        if(Velocity.x < 0) {
            if(sprite.FlipH == false) {
                sprite.FlipH = true;
            }
            if(legs.FlipH == false) {
                legs.FlipH = true;
            }
        }
        //right
        else if(Velocity.x > 0) {
            if(sprite.FlipH == true) {
                sprite.FlipH = false;
            }
            if(legs.FlipH == true) {
                legs.FlipH = false;
            }
        }
    }


    public virtual void Hit(Vector2 linearV, int damage) {
        if(IsDead == true) {
            return;
        }
        ApplyCentralImpulse(linearV);
        Health -= damage;
        if(Health <= 0) {
            IsDead = true;
            deathTimer.Start();
            legs.Animation = "idle";
            anim.Play("die");
        }
    }


    public virtual void OnDeathTimerTimeout() {
        QueueFree();
    }


}
