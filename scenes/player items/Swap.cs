using Godot;
using System;

public class Swap : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(Weapon.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(Weapon.GlobalPosition);
        Weapon.Teleport(playerPos);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
