using Godot;
using System;

public class Boost : WeaponItem
{
    private const int BOOST_SPEED = 250;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Throw(BOOST_SPEED, WeaponNode.GlobalPosition, WeaponNode.GlobalRotation);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
