using Godot;
using System;

public class SprayerProj : EnemyProj
{
    public override void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRotDeg = 0) {
        base.Spawn(lvl, globalPos, destination, globalRotDeg);
        ((Particles2D)GetNode("Particles2D")).GlobalRotation = 0;
    }


    public override void StopProjectile() {
        base.StopProjectile();
        sprite.GlobalRotationDegrees = (float)GD.RandRange(0,360);
    }


    public override void ReturnToPool() {
        base.ReturnToPool();
    }
    

}
