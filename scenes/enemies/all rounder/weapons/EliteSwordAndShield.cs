using Godot;
using System;

public class EliteSwordAndShield : AllRounderWeapon
{
    public void ThrowFlyBlob() {
        Anim.Play("throw_flyblob");
    }


    readonly String[] flyBlobs = {
        "flyblob_1", "flyblob_2", "flyblob_3"
    };


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "flyblob") {
            Godot.Collections.Array arr = new Godot.Collections.Array(flyBlobs);
            arr.Shuffle();
            EnemyProj flyblob = (EnemyProj)spawnScenes[(String)arr[0]].Instance();
            flyblob.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero);
        }
    }


    public override void FinishShooting(String animName = "")
    {
        base.FinishShooting();
    }


}
