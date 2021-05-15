using System;
using Godot;

public abstract class BaseAllRounder: Enemy
{
    //melee
    protected const int FORWARD_IMPULSE = 500;


    public virtual void MeleeAttack() {
        WeaponNode.MeleeAttack();
    }


    //ranged
    public virtual void Shoot() {
        WeaponNode.Shoot();
    }


    public virtual void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(WeaponNode.GlobalRotation) * FORWARD_IMPULSE);
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
