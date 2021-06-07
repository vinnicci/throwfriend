using Godot;
using System;

public class Boost : WeaponItem
{
    private const int BOOST_SPEED = 130;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Throw(BOOST_SPEED, WeaponNode.GlobalPosition, Vector2.Zero, WeaponNode.GlobalRotation);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
