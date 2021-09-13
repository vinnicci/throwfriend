using Godot;
using System;

public class Boost : WeaponItem
{
    const int BOOST_SPEED = 125;


    public override void ApplyEffect()
    {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid || Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        WeaponNode.Throw(BOOST_SPEED, WeaponNode.GlobalPosition, Vector2.Zero, WeaponNode.GlobalRotation);
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        if(WeaponNode.IsClone == false) {
            Cooldown.Start();
        }
    }


}
