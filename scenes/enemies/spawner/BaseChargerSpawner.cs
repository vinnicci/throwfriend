using Godot;
using System;

public abstract class BaseChargerSpawner : Enemy, ISpawner
{
    public void SpawnBlobCharger() {
        anim.Play("spawn_charger");
    }


    const int BLOB_SPAWN_FORCE = 1000;


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        for(int i = 0; i <= count - 1; i++) {
            if(packedSceneKey == "charger") {
                ISpawnable bInstance = (ISpawnable)spawnScenes[packedSceneKey].Instance();
                bInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
                ((RigidBody2D)bInstance).ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
            }
        }
    }


}
