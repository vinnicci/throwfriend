using Godot;
using System;

public class SuperThrow : PlayerItem
{
    public override void _Ready()
    {
    }


    //  public override void _Process(float delta)
    //  {
    //  }


    private const int EXTRA_STRENGTH = 200;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
    }


}
