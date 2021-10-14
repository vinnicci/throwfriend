using Godot;
using System;

public abstract class BaseCharger : Enemy
{
    protected bool charging = false;
    protected const int CHARGE_MIN_V = 100000;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        int lv = (int)LinearVelocity.LengthSquared();
        if(charging && lv <= CHARGE_MIN_V) {
            charging = false;
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        }
        else if(charging) {
            LeaveTrail(2);
        }
    }


    protected const int CHARGE_KNOCKBACK = 250;
    protected const int CHARGE_STRENGTH = 250;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
        if(charging == false) {
            return;
        }
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - GlobalPosition).Clamped(1) * CHARGE_KNOCKBACK, 1);
        }
    }


    public virtual void Charge() {
        Vector2 vec = LevelNode.GetPlayerPos();
        if(vec == Vector2.Zero) {
            return;
        }
        ApplyCentralImpulse((vec - GlobalPosition).Clamped(1) * CHARGE_STRENGTH);
        ContinuousCd = RigidBody2D.CCDMode.CastRay;
        charging = true;
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
    }


}