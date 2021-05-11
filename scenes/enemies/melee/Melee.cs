using Godot;
using System;

public class Melee : Enemy
{
    public override void OnEnemyBodyEntered(Godot.Object body)
    {
    }


    private const int FORWARD_IMPULSE = 500;


    public void Attack() {
        WeaponNode.Shoot();
    }


    public void ApplyCentralImpulseAct() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
    }


    public override void FinishAction() {
        base.FinishAction();
    }



}
