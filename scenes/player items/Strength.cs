using Godot;
using System;

public class Strength : PlayerItem
{
    const int EXTRA_STRENGTH = 50;
    const float SPEED_REDUCTION_BONUS = 0.35f;
    bool done = false;


    public override void InitEffect()
    {
        base.InitEffect();
        if(done) {
            return;
        }
        PlayerNode.ThrowStrength += EXTRA_STRENGTH;
        PlayerNode.knockbackMult -= 0.5f;
        PlayerNode.SnarkCarrySpeedReduction -= SPEED_REDUCTION_BONUS;
        PlayerNode.Speed = (int)(PlayerNode.speed - PlayerNode.speed * PlayerNode.SnarkCarrySpeedReduction);
        done = true;
    }


}
