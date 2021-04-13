using Godot;
using System;

public class Swap : PlayerItem
{
    public Weapon Weapon {get; set;}


    public override void _Ready()
    {
    }


    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustReleased("right_click")) {
            GD.Print("teleport!");
            ApplyEffect();
        }
    }


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.Weapon;
    }


    public override void ApplyEffect()
    {
        if(Weapon.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        base.ApplyEffect();
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(Weapon.GlobalPosition);
        Weapon.Teleport(playerPos);
    }


}
