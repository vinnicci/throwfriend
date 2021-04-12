using Godot;
using System;

public class ExtraDamage : WeaponItem
{
    public override void _Ready()
    {
    }


    //  public override void _Process(float delta)
    //  {
    //  }


    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode.Damage += 1;
    }
}
