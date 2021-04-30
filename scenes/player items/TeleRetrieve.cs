using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(Weapon.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        Weapon.Teleport(PlayerNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
