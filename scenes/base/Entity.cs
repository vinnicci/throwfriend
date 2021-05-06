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
    protected Node2D spriteNode;
    protected Godot.Collections.Array spriteChildren = new Godot.Collections.Array();


    public override void _Ready()
    {
        base._Ready();
        deathTimer = (Timer)GetNode("DeathTimer");
        hitCooldown = (Timer)GetNode("HitCooldown");
        anim = (AnimationPlayer)GetNode("Anim");
        spriteNode = (Node2D)GetNode("Sprite");
        foreach(Godot.Object spriteCh in spriteNode.GetChildren()) {
            if(spriteCh is Sprite) {
                spriteChildren.Add(spriteCh);
            }
        }
        legs = (AnimatedSprite)spriteNode.GetNode("Legs");
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
        Sprite sprite = (Sprite)spriteChildren[0];
        if(Velocity.x < 0) {
            if(sprite.FlipH == false) {
                foreach(Sprite spChild in spriteChildren) {
                    spChild.FlipH = true;
                }
            }
            if(legs.FlipH == false) {
                legs.FlipH = true;
            }
        }
        //right
        else if(Velocity.x > 0) {
            if(sprite.FlipH == true) {
                foreach(Sprite spChild in spriteChildren) {
                    spChild.FlipH = false;
                }
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
        ModifyHealth(-damage);
        if(Health <= 0) {
            IsDead = true;
            deathTimer.Start();
            legs.Animation = "idle";
            anim.Stop();
            anim.Play("die");
        }
        else if(Health > 0) {
            hitCooldown.Start();
        }
    }


    private void ModifyHealth(int val) {
        if(val < 0) {
            anim.Play("damaged");
        }
        Health += val;
        Health = Godot.Mathf.Clamp(Health, 0, 3);
    }


    public virtual void OnDeathTimerTimeout() {
        QueueFree();
    }


}
