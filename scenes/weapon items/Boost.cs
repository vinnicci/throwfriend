using Godot;
using System;

public class Boost : WeaponItem
{
    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Guided");
        incompatibilityList.Add("AutoRetrieve");
    }


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.LookAt(GetGlobalMousePosition());
        WeaponNode.Throw(PlayerNode.ThrowStrength, WeaponNode.GlobalPosition, Vector2.Zero, WeaponNode.GlobalRotation);
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        if(WeaponNode.IsClone == false) {
            Cooldown.Start();
        }
    }


}
