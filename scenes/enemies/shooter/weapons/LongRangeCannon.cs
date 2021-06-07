using Godot;
using System;

public class LongRangeCannon : ShooterWeapon
{
    public override void SpawnProj()
    {
        EnemyProj proj = (EnemyProj)projectile.Instance();
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition,
        ToLocal(ParentNode.LevelNode.GetPlayerPos()), spawnPoint.GlobalRotationDegrees);
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
