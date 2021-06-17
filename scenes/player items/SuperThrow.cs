using Godot;
using System;

public class SuperThrow : PlayerItem
{
    private const int EXTRA_STRENGTH = 125;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
    }


}
