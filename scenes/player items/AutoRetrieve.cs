using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}
    private bool isReturning = false;
    private const int speed = 100;
    

    public override void _Ready()
    {
    }


    //  public override void _Process(float delta)
    //  {
    //  }


    public override void _PhysicsProcess(float delta)
    {
        if(isReturning == true) {
            if(Weapon.Mode == RigidBody2D.ModeEnum.Rigid && Weapon.IsTakable == true) {
                Vector2 vec = (PlayerNode.GlobalPosition - Weapon.GlobalPosition).Clamped(1) * speed;
                Weapon.AppliedForce = vec;
            }
            else if(Weapon.GlobalPosition.DistanceSquaredTo(PlayerNode.GlobalPosition) <= 10000) {
                isReturning = false;
                Weapon.AppliedForce = Vector2.Zero;
                Weapon.SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
            }
        }
        else if(isReturning == false && Weapon.Mode == RigidBody2D.ModeEnum.Rigid && Weapon.IsTakable == true) {
            isReturning = true;
            Weapon.SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        Weapon = PlayerNode.Weapon;
        Weapon.AddChild(this);
    }


    // public override void ApplyEffect()
    // {
    //     base.ApplyEffect();
    //     isReturning = true;
    // }


}
