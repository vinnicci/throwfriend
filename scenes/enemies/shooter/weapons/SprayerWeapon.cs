using Godot;
using System;

public class SprayerWeapon : EnemyWeapon
{
    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey, count);
    }


}
