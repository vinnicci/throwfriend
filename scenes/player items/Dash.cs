using Godot;
using System;

public class Dash : PlayerItem
{
    const int DASH_STRENGTH = 300;


    public override void ApplyEffect()
    {
        Vector2 playerVNorm = PlayerNode.Velocity;
        if(Cooldown.IsStopped() == false || playerVNorm.LengthSquared() <= 0) {
            return;
        }
        base.ApplyEffect();
        Vector2 vec = playerVNorm.Normalized() * DASH_STRENGTH;
        PlayerNode.ApplyCentralImpulse(vec);
        PlayerNode.ContinuousCd = RigidBody2D.CCDMode.CastRay;
        PlayerNode.HitCooldown.Start(0.3f);
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Cooldown.IsStopped() == false && PlayerNode.HitCooldown.IsStopped() == false) {
            PlayerNode.LeaveTrail(2);
        }
    }


}
