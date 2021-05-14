using Godot;
using System;

public class SuperThrow : PlayerItem
{
    private const int EXTRA_STRENGTH = 250;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
    }


}
