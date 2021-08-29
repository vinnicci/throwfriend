using Godot;
using System;

public class FlyBlobCarrier : AllRounder
{
    public void SpawnFlyBlob() {
        if(Health <= 0) {
            return;
        }
        LevelNode.PlayerEngaging.Remove(Name);
        GD.Print("die: " + LevelNode.PlayerEngaging.Count);
        EmitSignal(nameof(Died));
        Health = 0;
        EnemyProj flyblob = (EnemyProj)spawnScenes["flyblob"].Instance();
        flyblob.Spawn(LevelNode, ((Node2D)GetNode("Sprite/Head")).GlobalPosition, Vector2.Zero);
    }


}
