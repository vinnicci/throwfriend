using System;
using Godot;

public abstract class BaseAllRounder: Enemy
{
    //melee
    protected const int FORWARD_IMPULSE = 250;


    public virtual void MeleeAttack() {
        WeaponNode.MeleeAttack();
    }


    public virtual void MeleeAttackBack() {
        ((BaseAllRounderWeapon)WeaponNode).MeleeAttackBack();
    }


    public virtual void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
    }


    //ranged
    protected const int BLASTER_RECOIL = 130;


    public virtual void Shoot() {
        WeaponNode.Shoot();
    }


    public virtual void ShootBack() {
        ((BaseAllRounderWeapon)WeaponNode).ShootBack();
    }


    public virtual void Recoil() {
        ApplyCentralImpulse(new Vector2(-1,0).Rotated(WeaponNode.GlobalRotation) * BLASTER_RECOIL);
    }


    //throw blobnade
    public virtual void ThrowBlob() {
        ((BaseAllRounderWeapon)WeaponNode).ThrowBlob();
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
