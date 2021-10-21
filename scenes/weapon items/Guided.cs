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
        incompatibilityList.Add("Boost");
    }


    const int HOME_MAGNITUDE = 125;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Active == false || WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid ||
        IsInstanceValid(WeaponNode) == false || IsInstanceValid(WeaponNode.PlayerNode) == false) {
            return;
        }
        Vector2 vec = (GetGlobalMousePosition() - WeaponNode.GlobalPosition).Normalized() * HOME_MAGNITUDE;
        WeaponNode.Velocity += vec;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Switch(true, Active == false);
    }


}
