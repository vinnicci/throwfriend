using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustReleased("right_click")) {
            ApplyEffect();
        }
    }


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(Weapon.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        base.ApplyEffect();
        Weapon.Teleport(PlayerNode.GlobalPosition);
    }


}
