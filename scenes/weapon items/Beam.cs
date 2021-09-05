using Godot;
using System;

public class Beam : WeaponItem
{
    public override void ApplyEffect()
    {
        if(Cooldown.IsStopped() == false || WeaponNode.Anim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.BeamAttack();
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        if(WeaponNode.IsClone == false) {
            Cooldown.Start();
        }
    }


}
