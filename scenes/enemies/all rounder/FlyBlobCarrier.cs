using Godot;
using System;

public class FlyBlobCarrier : AllRounder
{
    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


    public void SpawnFlyBlob() {
        if(Health <= 0) {
            return;
        }
        LevelNode.PlayerEngaging.Remove(Name);
        EmitSignal(nameof(Died));
        Health = 0;
        EnemyProj flyblob = (EnemyProj)spawnScenes["flyblob"].Instance();
        flyblob.Spawn(LevelNode, ((Node2D)GetNode("Sprite/Head")).GlobalPosition, Vector2.Zero);
    }


}
