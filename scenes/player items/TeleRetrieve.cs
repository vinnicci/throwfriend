using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void ApplyEffect()
    {
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        if(Weapon.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        Weapon.Teleport(PlayerNode.LevelNode, PlayerNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
