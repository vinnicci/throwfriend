using Godot;
using System;

public class BlobCharger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public void SpawnBlob() {
        anim.Play("spawn_blob");
    }


    private const int BLOB_SPAWN_FORCE = 1000;


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "blob") {
            for(int i = 0; i <= count - 1; i++) {
                Blob blobInstance = (Blob)spawnScenes[packedSceneKey].Instance();
                Node2D ai = (Node2D)spawnScenes["ai"].Instance();
                blobInstance.AddChild(ai);
                blobInstance.LevelNode = LevelNode;
                blobInstance.Spawn(blobInstance.LevelNode, GlobalPosition, Vector2.Zero);
                blobInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
            }
        }
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
