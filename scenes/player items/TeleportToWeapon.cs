using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    const float FAILURE_CHANCE = 10;


    public override void ApplyEffect()
    {
        if(WeaponNode != PlayerNode.WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        EmitSignal(nameof(Activated));
        Cooldown.Start();
        if(GD.RandRange(0, 100f) > FAILURE_CHANCE) {
            PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        }
    }


}
