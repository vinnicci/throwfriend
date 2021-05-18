using Godot;
using System;

public class Dash : PlayerItem
{
    private const int DASH_STRENGTH = 500;


    public override void ApplyEffect()
    {
        if(Cooldown.IsStopped() == false) {
            return;
        }
        base.ApplyEffect();
        Vector2 mousePos = new Vector2(GetGlobalMousePosition());
        Vector2 vec = (mousePos - PlayerNode.GlobalPosition).Clamped(1) * DASH_STRENGTH;
        PlayerNode.ApplyCentralImpulse(vec);
        PlayerNode.HitCooldown.Start(0.3f);
        EmitSignal("Activated");
        Cooldown.Start();
    }


}
