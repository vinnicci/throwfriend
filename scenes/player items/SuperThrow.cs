using Godot;
using System;

public class SuperThrow : PlayerItem
{
    const int EXTRA_STRENGTH = 50;
    bool done = false;


    public override void InitEffect()
    {
        base.InitEffect();
        if(done) {
            return;
        }
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
        PlayerNode.UpdateSnarkDmg();
        done = true;
    }


}
