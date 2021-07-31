using Godot;
using System;

public class SprayerStickyProj : SprayerProj
{
    Player player;


    public override bool OnEnemyProjBodyEntered(Godot.Object body) {
        player = (Player)body;
        return true;
    }


    public void OnSprayerAcidProjBodyExited(Godot.Object body) {
        player = default;
    }


    const float SLOW_EFFECT = 0.3f;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(IsInstanceValid(player) && player.Velocity.Length() > SLOW_EFFECT) {
            player.Velocity *= SLOW_EFFECT;
        }
    }


}
