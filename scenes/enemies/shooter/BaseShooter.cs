using Godot;
using System;

public abstract class BaseShooter : Enemy
{
    public void Shoot() {
        WeaponNode.Shoot();
    }


    public override void FinishAction(String actionName)
    {
        base.FinishAction(actionName);
    }


}
