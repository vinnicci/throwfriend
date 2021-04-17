using Godot;
using System;

public class SuperThrow : PlayerItem
{
    public override void _Ready()
    {
    }


    private const int EXTRA_STRENGTH = 200;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        PlayerNode.ThrowStrength -= EXTRA_STRENGTH;
    }


}