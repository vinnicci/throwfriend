using Godot;
using System;

public class Swap : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void ApplyEffect()
    {
        if(Weapon.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(Weapon.GlobalPosition);
        Weapon.Teleport(playerPos);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
