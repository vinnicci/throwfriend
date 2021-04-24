using Godot;
using System;

public class ExtraSpeed : PlayerItem
{
    const int EXTRA_SPEED = 350;


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode.Speed += EXTRA_SPEED;
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        PlayerNode.Speed -= EXTRA_SPEED;
    }


}
