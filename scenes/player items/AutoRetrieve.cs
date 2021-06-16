using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    private bool weapIsReturning = false;
    private const int RETRIEVE_SPEED = 130;
    

    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        incompatibilityList.Add("Guided");
    }


    private const int STOP_RETURN_DIST = 10000;


    public override void InitEffect()
    {
        base.InitEffect();
        WeaponNode = PlayerNode.WeaponNode;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(weapIsReturning == true) {
            Vector2 vec = (PlayerNode.GlobalPosition - WeaponNode.GlobalPosition).Clamped(1) * RETRIEVE_SPEED;
            WeaponNode.Velocity = vec;
            if(WeaponNode.CurrentState == Weapon.States.HELD) {
                weapIsReturning = false;
            }
        }
        else if(weapIsReturning == false && WeaponNode.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
    }


}
