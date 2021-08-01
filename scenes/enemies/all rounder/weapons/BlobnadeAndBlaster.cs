using Godot;
using System;

public class BlobnadeAndBlaster : AllRounderWeapon
{
    const int BLOBNADE_THROW_STRENGTH = 1000;


    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey);
    }


    public override void FinishShooting() {
        base.FinishShooting();
    }

    
}
