using Godot;
using System;

public class ShooterWeapon : EnemyWeapon
{
    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey, count);
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
