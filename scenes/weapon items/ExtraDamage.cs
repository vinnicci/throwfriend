using Godot;
using System;

public class ExtraDamage : WeaponItem
{
    const int EXTRA_DMG = 3;
    bool done = false;


    public override void InitEffect()
    {
        base.InitEffect();
        if(done == true || WeaponNode.IsClone == true) {
            return;
        }
        WeaponNode.Damage *= EXTRA_DMG;
        done = true;
    }


}
