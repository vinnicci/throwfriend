using Godot;
using System;

public class EliteBlobCharger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    //normal blobs
    public void SpawnBlob() {
        anim.Play("spawn_blob");
    }


    //teleporting blobs, 50% hp
    public void SpawnTeleportingBlob() {
        anim.Play("spawn_teleporting_blob");
    }


    //nuke blobs, 25% hp
    public void SpawnNukeBlob() {
        anim.Play("spawn_nuke_blob");
    }


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey, count);
        if(packedSceneKey == "blob" || packedSceneKey == "teleporting_blob" || packedSceneKey == "nuke_blob") {
            for(int i = 0; i <= count - 1; i++) {
                SpawnBlobInstance(packedSceneKey);
            }
        }
    }


    void SpawnBlobInstance(String packedSceneKey) {
        Blob blobInstance = (Blob)spawnScenes[packedSceneKey].Instance();
        blobInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        blobInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) *
        (float)GD.RandRange(500, 1000));
        blobInstance.ContinuousCd = RigidBody2D.CCDMode.CastRay;
    }


    public void SpawnBlobsOnDeath() {
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add("blob");
        arr.Add("teleporting_blob");
        arr.Add("nuke_blob");
        for(int i = 0; i <= 2; i++) {
            arr.Shuffle();
            SpawnBlobInstance((String)arr[0]);
        }
    }


}
