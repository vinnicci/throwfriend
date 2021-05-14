using Godot;
using System;

public class Charger : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("charge", (float)actCooldown[0]);
    }


    private bool charging = false;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(charging == true && LinearVelocity.LengthSquared() <= 5625) {
            charging = false;
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        }
    }


    private const int CHARGE_KNOCKBACK = 100;
    private const int CHARGE_STRENGTH = 500;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
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


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
