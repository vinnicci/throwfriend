using Godot;
using System;

public class Charger : Enemy
{
    private bool charging = false;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(charging == true && LinearVelocity.LengthSquared() <= 5625) {
            charging = false;
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        }
    }


    const int CHARGE_KNOCKBACK = 100;
    const int CHARGE_STRENGTH = 550;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        if(charging == false) {
            return;
        }
        if(body == PlayerNode) {
            PlayerNode.Hit((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * CHARGE_KNOCKBACK, 1);
        }
    }


    public void Charge() {
        ApplyCentralImpulse((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * CHARGE_STRENGTH);
        charging = true;
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
    }


    public override void FinishAction() {
        base.FinishAction();
    }

    
}
