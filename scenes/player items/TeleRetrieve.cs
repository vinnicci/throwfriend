using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public override void ApplyEffect()
    {
        if(WeaponNode != PlayerNode.WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        if(WeaponNode.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Teleport(PlayerNode.LevelNode, PlayerNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
