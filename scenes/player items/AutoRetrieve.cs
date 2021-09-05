using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    bool weapIsReturning = false;
    const int RETRIEVE_SPEED = 125;
    

    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        incompatibilityList.Add("Guided");
    }


    const int STOP_RETURN_DIST = 10000;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(active == false) {
            return;
        }
        if(WeaponNode != PlayerNode.WeaponNode) {
            WeaponNode = PlayerNode.WeaponNode;
        }
        if(weapIsReturning) {
            Vector2 vec = (PlayerNode.GlobalPosition - WeaponNode.GlobalPosition).Normalized() * RETRIEVE_SPEED;
            WeaponNode.Velocity = vec;
            if(WeaponNode.CurrentState == Weapon.States.HELD) {
                weapIsReturning = false;
            }
        }
        else if(weapIsReturning == false && WeaponNode.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
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
