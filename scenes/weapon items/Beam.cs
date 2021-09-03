using Godot;
using System;

public class Beam : WeaponItem
{
    public override void ApplyEffect()
    {
        if(Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.BeamAttack();
        EmitSignal(nameof(Activated));
        Cooldown.Start();
    }


}
