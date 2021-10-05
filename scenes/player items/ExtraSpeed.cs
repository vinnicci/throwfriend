using Godot;
using System;

public class ExtraSpeed : PlayerItem
{
    const int EXTRA_SPEED = 100;
    bool done = false;
    

    public override void InitEffect()
    {
        base.InitEffect();
        if(done) {
            return;
        }
        PlayerNode.ChangeEntityBaseStats(-1, PlayerNode.speed + EXTRA_SPEED);
        PlayerNode.Speed = (int)(PlayerNode.speed - PlayerNode.speed * PlayerNode.SnarkCarrySpeedReduction);
        done = true;
    }


}
