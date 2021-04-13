using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}
    private bool weapIsReturning = false;
    private const int speed = 100;
    

    public override void _Ready()
    {
    }


    public override void _PhysicsProcess(float delta)
    {
        if(weapIsReturning == true) {
            if(Weapon.Mode == RigidBody2D.ModeEnum.Rigid && Weapon.IsTakable == true) {
                Vector2 vec = (PlayerNode.GlobalPosition - Weapon.GlobalPosition).Clamped(1) * speed;
                Weapon.AppliedForce = vec;
            }
            else if(Weapon.GlobalPosition.DistanceSquaredTo(PlayerNode.GlobalPosition) <= 10000) {
                weapIsReturning = false;
                Weapon.AppliedForce = Vector2.Zero;
                Weapon.SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
            }
        }
        else if(weapIsReturning == false && Weapon.Mode == RigidBody2D.ModeEnum.Rigid && Weapon.IsTakable == true) {
            weapIsReturning = true;
            Weapon.SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        Weapon = PlayerNode.Weapon;
    }


}
