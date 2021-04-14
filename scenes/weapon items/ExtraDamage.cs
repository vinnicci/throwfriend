using Godot;
using System;

public class ExtraDamage : WeaponItem
{
    public override void _Ready()
    {
    }


    private const int DMG_MULT = 2;


    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode.Damage *= DMG_MULT;
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        WeaponNode.Damage /= DMG_MULT;
    }


}
