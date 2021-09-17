using Godot;
using System;

public class EliteMeleeShielded : AllRounder
{
    public override void MeleeAttackBack() {
        base.MeleeAttackBack();
    }


    public override void AttackImpulse()
    {
        base.AttackImpulse();
    }


    public void TeleAttack() {
        Vector2 vec = LevelNode.GetPlayerPos() -
        new Vector2(50,0).Rotated(Mathf.Deg2Rad((float)GD.RandRange(0, 360)));
        Teleport(LevelNode, vec);
    }


    public void ThrowFlyBlob() {
        ((EliteSwordAndShield)WeaponNode).ThrowFlyBlob();
    }


}
