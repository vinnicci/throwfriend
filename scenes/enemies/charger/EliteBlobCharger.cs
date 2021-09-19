using Godot;
using System;

public class EliteBlobCharger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        if(Modulate.a < 1) {
            Modulate = new Color(1,1,1,1);
        }
        base.Charge();
    }


    public override bool IsActReady(string actionName)
    {
        if(base.IsActReady(actionName)) {
            if(actionName == "invisible") {
                if(Visible) {
                    return true;
                }
                return false;
            }
            if(actionName == "charge") {
                if(Visible == false) {
                    Modulate = new Color(1,1,1,1);
                    Visible = true;
                }
            }
            return true;
        }
        return false;
    }


    public void Invisible() {
        anim.Play("invisible");
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
