using Godot;
using System;

public abstract class EnemyWeapon : Node2D, ISpawner
{
    [Export] public Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}
    
    public Enemy ParentNode {get; set;}

    protected Position2D spawnPoint;
    public AnimationPlayer Anim {get; set;}


    public override void _Ready()
    {
        base._Ready();
        spawnPoint = (Position2D)GetNode("Sprites/ProjSpawn");
        Anim = (AnimationPlayer)GetNode("Anim");
    }


    //generic melee action
    public virtual void MeleeAttack() {
        Anim.Play("melee_attack");
    }


    //generic ranged action
    public virtual void Shoot() {
        Anim.Play("shoot");
    }


    public virtual void SpawnInstance(String packedSceneKey, int count = 1) {
        if(packedSceneKey == "proj") {
            SpawnProj();
        }
    }


    //used by range attack anim to create projectiles
    public virtual void SpawnProj() {
        EnemyProj proj = (EnemyProj)spawnScenes["proj"].Instance();
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero, spawnPoint.GlobalRotationDegrees);
    }


    public virtual void FinishShooting() {
        Anim.Play("idle");
    }


}
