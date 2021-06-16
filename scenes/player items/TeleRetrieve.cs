using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState != Weapon.States.INACTIVE || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Teleport(PlayerNode.LevelNode, PlayerNode.GlobalPosition);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
