using Godot;
using System;

public class SuperThrow : PlayerItem
{
    const int EXTRA_STRENGTH = 75;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
    }


}
