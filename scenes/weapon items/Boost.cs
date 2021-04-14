using Godot;
using System;

public class Boost : WeaponItem
{
    private Timer cooldown;


    public override void _Ready()
    {
        cooldown = (Timer)GetNode("Cooldown");
    }


    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustReleased("right_click")) {
            ApplyEffect();
        }
    }


    private const int BOOST_SPEED = 150;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Throw(BOOST_SPEED, WeaponNode.GlobalPosition, WeaponNode.GlobalRotation);
        cooldown.Start();
    }


}
