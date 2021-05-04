using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void ApplyEffect()
    {
        if(Weapon.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        Weapon.Teleport(PlayerNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
