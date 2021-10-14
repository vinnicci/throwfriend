using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    const float FAILURE_CHANCE = 5;


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false ||
        PlayerNode.TeleportAnim.IsPlaying() || WeaponNode.TeleportAnim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) > FAILURE_CHANCE) {
            WeaponNode.Teleport(PlayerNode.LevelNode, PlayerNode.GlobalPosition);
        }
        else {
            PlaySoundEffect("TeleportFailed");
        }
    }


}
