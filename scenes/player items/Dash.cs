using Godot;
using System;

public class Dash : PlayerItem
{
    const int DASH_STRENGTH = 300;
    
    public bool IsDashing {get; set;}


    public override void ApplyEffect()
    {
        Vector2 playerVNorm = PlayerNode.Velocity;
        if(Cooldown.IsStopped() == false || playerVNorm.LengthSquared() <= 0 || IsReady() == false) {
            return;
        }
        base.ApplyEffect();
        Vector2 vec = playerVNorm.Normalized() * DASH_STRENGTH;
        PlayerNode.ApplyCentralImpulse(vec);
        PlayerNode.ContinuousCd = RigidBody2D.CCDMode.CastRay;
        PlayerNode.HitCooldown.Start(0.3f);
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        IsDashing = true;
        PlaySoundEffect("Dash");
        Cooldown.Start();
    }


    bool IsReady() {
        return (
            (this == PlayerNode.Item1 && PlayerNode.Item2 is Dash && ((Dash)PlayerNode.Item2).IsDashing == false) ||
            (this == playerNode.Item2 && PlayerNode.Item1 is Dash && ((Dash)PlayerNode.Item1).IsDashing == false)
        );
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Cooldown.IsStopped() == false && PlayerNode.HitCooldown.IsStopped() == false) {
            PlayerNode.LeaveTrail(2);
        }
        else if(Cooldown.IsStopped() == false && PlayerNode.HitCooldown.IsStopped() && IsDashing) {
            IsDashing = false;
        }
    }


}
