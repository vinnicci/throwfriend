using Godot;
using System;

public class Guided : WeaponItem
{
    public override void _Ready()
    {
        incompatibilityList.Add("res://scenes/weapon items/Homing.cs");
        incompatibilityList.Add("res://scenes/weapon items/Guided.cs");
    }


    private const int HOME_MAGNITUDE = 250;


    public override void _PhysicsProcess(float delta)
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        Vector2 vec = (GetGlobalMousePosition() - WeaponNode.GlobalPosition).Clamped(1) * HOME_MAGNITUDE;
        WeaponNode.Velocity += vec;
    }


}
