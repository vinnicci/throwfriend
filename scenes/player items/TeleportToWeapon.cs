using Godot;
using System;

public class TeleportToWeapon : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
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
        PlayerNode.Teleport(Weapon.GlobalPosition);
    }


}
