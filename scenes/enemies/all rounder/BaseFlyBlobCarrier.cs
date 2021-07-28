using Godot;
using System;

public class BaseFlyBlobCarrier : BaseAllRounder
{
    public void SpawnFlyBlob() {
        if(IsDead == true) {
            return;
        }
        EmitSignal(nameof(Entity.Died));
        IsDead = true;
        EnemyProj flyblob = (EnemyProj)spawnScenes["flyblob"].Instance();
        flyblob.Spawn(LevelNode, ((Node2D)GetNode("Sprite/Head")).GlobalPosition, Vector2.Zero);
    }


}
