using Godot;
using System;

public abstract class ShooterWeapon : EnemyWeapon
{
    public override void Shoot()
    {
        base.Shoot();
    }


    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey, count);
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


}
