using Godot;
using System;

public class Dash : PlayerItem
{
    private Timer cooldown;


    public override void _Ready()
    {
        base._Ready();
        cooldown = (Timer)GetNode("Cooldown");
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Input.IsActionJustReleased("right_click")) {
            ApplyEffect();
        }
    }
    

    private const int DASH_STRENGTH = 500;


    public override void ApplyEffect()
    {
        if(cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        Vector2 mousePos = new Vector2(GetGlobalMousePosition());
        Vector2 vec = (mousePos - PlayerNode.GlobalPosition).Clamped(1) * DASH_STRENGTH;
        PlayerNode.ApplyCentralImpulse(vec);
        cooldown.Start();
    }


}
