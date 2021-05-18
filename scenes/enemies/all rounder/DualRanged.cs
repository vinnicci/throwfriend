using Godot;
using System;

public class DualRanged : AllRounder
{
    public override void Shoot()
    {
        base.Shoot();
    }


    public void Shoot2() {
        ((DualBlaster)WeaponNode).Shoot2();
    }


    public override void Recoil()
    {
        base.Recoil();
    }


    public override void FinishAction(string actionName)
    {
        base.FinishAction(actionName);
    }

    
}
