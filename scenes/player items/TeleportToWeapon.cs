using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void ApplyEffect()
    {
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        if(Weapon.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        PlayerNode.Teleport(PlayerNode.LevelNode, Weapon.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
