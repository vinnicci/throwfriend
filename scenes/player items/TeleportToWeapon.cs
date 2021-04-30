using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    public Weapon Weapon {get; set;}

    
    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(Weapon.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        PlayerNode.Teleport(Weapon.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
