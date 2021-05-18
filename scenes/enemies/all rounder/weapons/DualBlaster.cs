using Godot;
using System;

public class DualBlaster : AllRounderWeapon
{
    public void Shoot2() {
        anim.Play("shoot_2");
    }


    public override void SpawnProj()
    {
        base.SpawnProj();
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }
}
