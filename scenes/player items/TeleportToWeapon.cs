using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    const float FAILURE_CHANCE = 10;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false ||
        PlayerNode.TeleportAnim.IsPlaying() || WeaponNode.TeleportAnim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) > FAILURE_CHANCE) {
            PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        }
    }


}
