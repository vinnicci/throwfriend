using Godot;
using System;

public class BlobChargerSpawner : Enemy, ISpawner
{
    public void SpawnBlobCharger() {
        anim.Play("spawn_blob_charger");
    }


    const int BLOB_SPAWN_FORCE = 1000;


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "blob_charger") {
            BlobCharger bInstance = (BlobCharger)spawnScenes[packedSceneKey].Instance();
            bInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
            bInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
        }
    }


    public override void FinishAction(string actionName)
    {
        base.FinishAction(actionName);
    }


}
