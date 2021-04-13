using Godot;
using System;

public class TeleRetrieve : PlayerItem
{
    public Weapon Weapon {get; set;}

    
    public override void _Ready()
    {
    }


//  public override void _Process(float delta)
//  { 
//  }


    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustReleased("right_click")) {
            ApplyEffect();
        }
    }


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.Weapon;
    }


    public override void ApplyEffect()
    {
        if(Weapon.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        base.ApplyEffect();
        Weapon.Teleport(PlayerNode.GlobalPosition);
    }


}
