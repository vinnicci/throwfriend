using Godot;
using System;

public class ShooterWeapon : EnemyWeapon
{
    public override void Shoot()
    {
        base.Shoot();
    }


    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey, count);
    }


}
