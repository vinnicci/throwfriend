using Godot;
using System;

public class SprayerStickyProj : SprayerProj
{
    Player player;


    public override void OnEnemyProjBodyEntered(Godot.Object body) {
        player = (Player)body;
    }


    public void OnSprayerAcidProjBodyExited(Godot.Object body) {
        player = default;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(IsInstanceValid(player) == true) {
            player.Velocity *= 0.5f;
        }
    }


}
