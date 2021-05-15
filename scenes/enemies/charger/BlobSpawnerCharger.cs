using Godot;
using System;

public class BlobSpawnerCharger : BaseCharger
{
    [Export] private PackedScene blob;
    [Export] private PackedScene aiNode;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public void SpawnBlob() {
        anim.Play("spawn_blob");
    }


    public void SpawnBlobsOnDeath() {
        InstanceBlob();
        InstanceBlob();
        InstanceBlob();
        InstanceBlob();
        InstanceBlob();
    }


    private const int BLOB_SPAWN_FORCE = 2000;


    public void InstanceBlob() {
        Blob blobInstance = (Blob)blob.Instance();
        Node2D ai = (Node2D)aiNode.Instance();
        blobInstance.AddChild(ai);
        ai.Call("init_properties", LevelNode, blobInstance);
        LevelNode.SpawnEnemy(blobInstance, GlobalPosition);
        blobInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
