using Godot;
using System;

public class Shooter : Enemy
{
    public void Shoot() {
        WeaponNode.Shoot();
    }


    public override void FinishAction(String actionName)
    {
        base.FinishAction(actionName);
    }


}
