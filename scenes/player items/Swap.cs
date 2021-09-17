using Godot;
using System;

public class Swap : PlayerItem
{
    const float FAILURE_CHANCE = 5;


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false ||
        PlayerNode.TeleportAnim.IsPlaying() || WeaponNode.TeleportAnim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) <= FAILURE_CHANCE) {
            return;
        }
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        if(PlayerNode.WeaponNode != WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        WeaponNode.Teleport(PlayerNode.LevelNode, playerPos);
        Explode();
    }


    async void Explode() {
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if(IsInstanceValid(WeaponNode.Item1) && WeaponNode.Item1 is Explosive) {
            ((Explosive)WeaponNode.Item1).Explode(null);
        }
        else if(IsInstanceValid(WeaponNode.Item2) && WeaponNode.Item2 is Explosive) {
            ((Explosive)WeaponNode.Item2).Explode(null);
        }
    }


}
