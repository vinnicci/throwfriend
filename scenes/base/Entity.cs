using Godot;
using System;

public abstract class Entity : RigidBody2D, IHealthModifiable, ITeleportable, ISpawnable
{
    [Export] protected int speed = 250;
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

    protected AnimationPlayer anim;
    protected Node2D spriteNode;
    private Node2D hud;
    private HealthHUD healthHUD;


    public override void _Ready()
    {
        base._Ready();
        hitCooldown = (Timer)GetNode("HitCooldown");
        spriteNode = (Node2D)GetNode("Sprite");
        Speed = speed;
        Health = health;
        IsDead = false;
        hud = (Node2D)GetNode("HUD");
        healthHUD = (HealthHUD)hud.GetNode("Health");
        healthHUD.ParentNode = this;
        anim = (AnimationPlayer)GetNode("Anim");
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


    public virtual void AdjustSprites() {
        //running
        if(anim.CurrentAnimation != "run" && anim.CurrentAnimation != "idle") {
            return;
        }
        if(Velocity == Vector2.Zero) {
            anim.Play("idle");
        }
        else {
            anim.Play("run");
        }
        if((Velocity.x < 0 && spriteNode.Scale.x > 0) || (Velocity.x > 0 && spriteNode.Scale.x < 0)) {
            Vector2 scale = new Vector2(-1,1);
            spriteNode.Scale *= scale;
        }
    }


    public virtual bool Hit(Vector2 knockback, int damage) {
        if(IsDead == true || HitCooldown.IsStopped() == false) {
            return false;
        }
        ApplyCentralImpulse(knockback);
        Health -= damage;
        if(Health <= 0) {
            IsDead = true;
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


}
