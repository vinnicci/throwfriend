using Godot;
using System;

public class Boost : WeaponItem
{
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


    private const int BOOST_SPEED = 150;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Throw(BOOST_SPEED, WeaponNode.GlobalPosition, WeaponNode.GlobalRotation);
    }


}
