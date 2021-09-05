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


    const int HOME_MAGNITUDE = 150;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(active == false || IsInstanceValid(WeaponNode) == false) {
            return;
        }
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || IsInstanceValid(WeaponNode.PlayerNode) == false) {
            return;
        }
        Vector2 vec = (GetGlobalMousePosition() - WeaponNode.GlobalPosition).Normalized() * HOME_MAGNITUDE;
        WeaponNode.Velocity += vec;
    }


    bool active = false;


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        if(active == false) {
            active = true;
            EmitSignal(nameof(Activated), -1);
        }
        else if(active) {
            active = false;
            EmitSignal(nameof(Activated), 0);
        }
    }


}
