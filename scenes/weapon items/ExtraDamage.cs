using Godot;
using System;

public class ExtraDamage : WeaponItem
{
    private const int EXTRA_DMG = 2;


    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode.Damage += EXTRA_DMG;
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        WeaponNode.Damage -= EXTRA_DMG;
    }


}
