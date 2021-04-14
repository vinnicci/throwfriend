using Godot;
using System;

public class Swap : PlayerItem
{
    public Weapon Weapon {get; set;}
    private Timer cooldown;


    public override void _Ready()
    {
        cooldown = (Timer)GetNode("Cooldown");
    }


    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustReleased("right_click")) {
            ApplyEffect();
        }
    }


    public override void InitEffect() {
        base.InitEffect();
        Weapon = PlayerNode.WeaponNode;
    }


    public override void ApplyEffect()
    {
        if(Weapon.CurrentState == Weapon.States.HELD || cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(Weapon.GlobalPosition);
        Weapon.Teleport(playerPos);
        cooldown.Start();
    }


}
