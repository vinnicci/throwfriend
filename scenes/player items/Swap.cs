using Godot;
using System;

public class Swap : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void ApplyEffect()
    {
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        if(Weapon.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(PlayerNode.LevelNode, Weapon.GlobalPosition);
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        Weapon.Teleport(PlayerNode.LevelNode, playerPos);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
