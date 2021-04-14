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
            if(Weapon.CurrentState == Weapon.States.INACTIVE) {
                Vector2 vec = (PlayerNode.GlobalPosition - Weapon.GlobalPosition).Clamped(1) * speed;
                Weapon.Velocity = vec;
            }
            else if(Weapon.GlobalPosition.DistanceSquaredTo(PlayerNode.GlobalPosition) <= 10000) {
                weapIsReturning = false;
                Weapon.AppliedForce = Vector2.Zero;
            }
        }
        else if(weapIsReturning == false && Weapon.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


}
