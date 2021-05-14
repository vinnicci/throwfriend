using Godot;
using System;

public class Melee : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("attack", (float)actCooldown[0]);
    }


    private const int FORWARD_IMPULSE = 500;


    public void Attack() {
        WeaponNode.Shoot();
    }


    public void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }



}
