using Godot;
using System;

public class LongRangeCannon : ShooterWeapon
{
    public override void SpawnInstance(string packedSceneKey, int count = 1)
    {
        base.SpawnInstance(packedSceneKey, count);
    }


    public override void SpawnProj()
    {
        EnemyProj proj = (EnemyProj)spawnScenes["proj"].Instance();
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition,
        ToLocal(ParentNode.LevelNode.GetPlayerPos()), spawnPoint.GlobalRotationDegrees);
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
