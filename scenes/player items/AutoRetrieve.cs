using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}
    
    private bool weapIsReturning = false;
    private const int RETRIEVE_SPEED = 200;
    

    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        incompatibilityList.Add("Guided");
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(weapIsReturning == true) {
            if(Weapon.CurrentState == Weapon.States.INACTIVE) {
                Vector2 vec = (PlayerNode.GlobalPosition - Weapon.GlobalPosition).Clamped(1) * RETRIEVE_SPEED;
                Weapon.Velocity = vec;
                if(Weapon.GetCollisionMaskBit(Global.BIT_MASK_CHAR) == false) {
                    Weapon.SetCollisionMaskBit(Global.BIT_MASK_CHAR, true);
                }
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
