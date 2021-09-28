using Godot;
using System;

public class SuperThrow : PlayerItem
{
    const int EXTRA_STRENGTH = 50;
    const int EXTRA_DMG = 1;
    bool done = false;


    public override void InitEffect()
    {
        base.InitEffect();
        if(done) {
            return;
        }
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
        PlayerNode.SnarkDmgMult += 1;
        done = true;
    }


}
