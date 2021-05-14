using Godot;
using System;

public class MeleeBlobnadier : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("attack", (float)actCooldown[0]);
        InitAct("throw_blob", (float)actCooldown[1]);
    }


    private const int FORWARD_IMPULSE = 500;


    public void Attack() {
        WeaponNode.Shoot();
    }


    public void ThrowBlob() {
        IBlobnade weap = (IBlobnade)WeaponNode;
        weap.ThrowBlob();
    }


    public void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }


}
