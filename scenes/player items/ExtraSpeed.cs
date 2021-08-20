using Godot;
using System;

public class ExtraSpeed : PlayerItem
{
    const int EXTRA_SPEED = 150;
    bool done = false;
    

    public override void InitEffect()
    {
        base.InitEffect();
        if(done) {
            return;
        }
        PlayerNode.Speed += EXTRA_SPEED;
        done = true;
    }


}
