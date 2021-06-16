using Godot;
using System;

public class Swap : PlayerItem
{
    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        if(PlayerNode.WeaponNode != WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        WeaponNode.Teleport(PlayerNode.LevelNode, playerPos);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
