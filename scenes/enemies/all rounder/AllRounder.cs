using System;
using Godot;

public class AllRounder: Enemy
{
    //melee
    protected const int FORWARD_IMPULSE = 500;


    public virtual void MeleeAttack() {
        WeaponNode.MeleeAttack();
    }


    public virtual void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
    }


    //ranged
    protected const int BLASTER_RECOIL = 250;


    public virtual void Shoot() {
        WeaponNode.Shoot();
    }


    public virtual void Recoil() {
        ApplyCentralImpulse(new Vector2(-1,0).Rotated(WeaponNode.GlobalRotation) * BLASTER_RECOIL);
    }


    //throw blobnade
    public virtual void ThrowBlob() {
        AllRounderWeapon weap = (AllRounderWeapon)WeaponNode;
        weap.ThrowBlob();
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
