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


    private const int BLOB_SPAWN_FORCE = 1000;


    public void InstanceBlob(int count) {
        for(int i = 1; i <= count; i++) {
            Blob blobInstance = (Blob)blob.Instance();
            Node2D ai = (Node2D)aiNode.Instance();
            blobInstance.AddChild(ai);
            ai.Call("init_properties", LevelNode, blobInstance);
            blobInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
            blobInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
        }
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
