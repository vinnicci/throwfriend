using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}
    
    private bool weapIsReturning = false;
    private const int RETRIEVE_SPEED = 130;
    

    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        incompatibilityList.Add("Guided");
    }


    private const int STOP_RETURN_DIST = 10000;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(PlayerNode.WeaponNode != Weapon) {
            Weapon = PlayerNode.WeaponNode;
        }
        if(weapIsReturning == true) {
            if(Weapon.CurrentState == Weapon.States.INACTIVE) {
                Vector2 vec = (PlayerNode.GlobalPosition - Weapon.GlobalPosition).Clamped(1) * RETRIEVE_SPEED;
                Weapon.Velocity = vec;
                if(Weapon.GetCollisionMaskBit(Global.BIT_MASK_ENEMY) == false) {
                    Weapon.SetCollisionMaskBit(Global.BIT_MASK_ENEMY, true);
                }
            }
            else if(Weapon.GlobalPosition.DistanceSquaredTo(PlayerNode.GlobalPosition) <= STOP_RETURN_DIST) {
                weapIsReturning = false;
                Weapon.AppliedForce = Vector2.Zero;
                if(Weapon.GetCollisionMaskBit(Global.BIT_MASK_ENEMY) == true) {
                    Weapon.SetCollisionMaskBit(Global.BIT_MASK_ENEMY, false);
                }
            }
        }
        else if(weapIsReturning == false && Weapon.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
    }


}
