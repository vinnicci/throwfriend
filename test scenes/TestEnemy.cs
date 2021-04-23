using Godot;
using System;

public class TestEnemy : Enemy
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


    public override void DoAction() {
        //charge player
        base.DoAction();
        ApplyCentralImpulse((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * 500);
        charging = true;
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true); 
    }


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
        if(charging == false) {
            return;
        }
        if(body == PlayerNode) {
            PlayerNode.Hit((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * 50, 1);
        }
    }

    
}
