using Godot;
using System;

public class LongRangeCannon : ShooterWeapon
{
    public override void Shoot()
    {
        base.Shoot();
    }


    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey, count);
    }


    public override void SpawnProj()
    {
        //EnemyProj proj = (EnemyProj)spawnScenes["proj"].Instance();
        EnemyProj proj = (EnemyProj)ParentNode.LevelNode.GetPooledObj(spawnScenes["proj"]);
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition,
        ToLocal(ParentNode.LevelNode.GetPlayerPos()), spawnPoint.GlobalRotationDegrees);
    }


}
