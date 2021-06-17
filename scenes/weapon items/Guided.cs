using Godot;
using System;

public class Guided : WeaponItem
{
    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Homing");
        incompatibilityList.Add("Guided");
        incompatibilityList.Add("AutoRetrieve");
    }


    private const int HOME_MAGNITUDE = 125;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || IsInstanceValid(WeaponNode.PlayerNode) == false) {
            return;
        }
        Vector2 vec = (GetGlobalMousePosition() - WeaponNode.GlobalPosition).Clamped(1) * HOME_MAGNITUDE;
        WeaponNode.Velocity += vec;
    }


}
