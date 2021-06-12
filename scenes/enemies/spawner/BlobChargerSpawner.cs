using Godot;
using System;

public class BlobChargerSpawner : Enemy
{
    [Export] PackedScene blobcharger;


    public void SpawnBlobCharger() {
        anim.Play("spawn_blob_charger");
    }


    const int BLOB_SPAWN_FORCE = 1000;


    public void InstanceBlobCharger() {
        BlobCharger bInstance = (BlobCharger)blobcharger.Instance();
        bInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        bInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
    }


    public override void FinishAction(string actionName)
    {
        base.FinishAction(actionName);
    }


}
