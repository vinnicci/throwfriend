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
            PlaySoundEffect("ThrowBlob");
            Godot.Collections.Array arr = new Godot.Collections.Array(flyBlobs);
            arr.Shuffle();
            //EnemyProj flyblob = (EnemyProj)spawnScenes[(String)arr[0]].Instance();
            EnemyProj flyblob = (EnemyProj)ParentNode.LevelNode.GetPooledObj(spawnScenes[(String)arr[0]]);
            flyblob.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero);
        }
    }


}
