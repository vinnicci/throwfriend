using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    public override void ApplyEffect()
    {
        if(WeaponNode != PlayerNode.WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
