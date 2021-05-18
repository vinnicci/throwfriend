using Godot;
using System;

public abstract class Entity : RigidBody2D, IHealthModifiable, ITeleportable, ISpawnable
{
    [Export] protected int speed = 500;
    public int Speed {get; set;}
    [Export] protected int health = 1;
    public int Health {get; set;}
    public Vector2 Velocity {get; protected set;}
    public bool IsDead {get; set;}
    protected Timer hitCooldown;
    public Timer HitCooldown {
        get {
            return hitCooldown;
        }
        set {
            return;
        }
    }

    protected Timer deathTimer;
    protected AnimationPlayer anim;
    protected AnimatedSprite legs;
    protected Node2D spriteNode;
    protected Godot.Collections.Array spriteChildren = new Godot.Collections.Array();
    private Node2D hud;
    private HealthHUD healthHUD;


    public override void _Ready()
    {
        base._Ready();
        deathTimer = (Timer)GetNode("DeathTimer");
        hitCooldown = (Timer)GetNode("HitCooldown");
        anim = (AnimationPlayer)GetNode("Anim");
        spriteNode = (Node2D)GetNode("Sprite");
        InitSpriteChildren(spriteNode);
        legs = (AnimatedSprite)spriteNode.GetNode("Legs");
        Speed = speed;
        Health = health;
        IsDead = false;
        hud = (Node2D)GetNode("HUD");
        healthHUD = (HealthHUD)hud.GetNode("Health");
        healthHUD.ParentNode = this;
    }


    private void InitSpriteChildren(Node2D node) {
        foreach(Godot.Object spriteCh in node.GetChildren()) {
            if(spriteCh is Sprite) {
                spriteChildren.Add(spriteCh);
                Node2D sNode = (Node2D)spriteCh;
                if(sNode.GetChildren().Count != 0) {
                    InitSpriteChildren(sNode);
                }
            }
        }
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        hud.GlobalRotation = 0;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        int lv = (int)LinearVelocity.LengthSquared();
        if(ContinuousCd == RigidBody2D.CCDMode.Disabled && lv > Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.CastRay;
        }
        else if(ContinuousCd == RigidBody2D.CCDMode.CastRay && lv <= Global.CCD_MAX) {
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
        AdjustSprites();
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity.Clamped(1) * Speed);
        Velocity = Vector2.Zero;
    }


    private void AdjustSprites() {
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
                    spChild.Offset *= new Vector2(-1,1);
                    spChild.FlipH = true;
                }
            }
            if(legs.FlipH == false) {
                legs.Offset *= new Vector2(-1,1);
                legs.FlipH = true;
            }
        }
        //right
        else if(Velocity.x > 0) {
            if(sprite.FlipH == true) {
                foreach(Sprite spChild in spriteChildren) {
                    spChild.Offset *= new Vector2(-1,1);
                    spChild.FlipH = false;
                }
            }
            if(legs.FlipH == true) {
                legs.Offset *= new Vector2(-1,1);
                legs.FlipH = false;
            }
        }
    }


    public virtual bool Hit(Vector2 knockback, int damage) {
        if(IsDead == true) {
            return false;
        }
        ApplyCentralImpulse(knockback);
        Health -= damage;
        if(Health <= 0) {
            IsDead = true;
            deathTimer.Start();
            legs.Animation = "idle";
            anim.Stop();
            anim.Play("die");
            SetCollisionLayerBit(Global.BIT_MASK_ENEMY, false);
            SetCollisionLayerBit(Global.BIT_MASK_PLAYER, false);
        }
        Health = Godot.Mathf.Clamp(Health, 0, health);
        healthHUD.UpdateHealth();
        return true;
    }


    public void Spawn(Level lvl, Vector2 globalPos, float globalRot = 0) {
        lvl.Spawn(this, globalPos, globalRot);
    }


    public virtual void OnDeathTimerTimeout() {
        anim.Play("die");
    }


}
