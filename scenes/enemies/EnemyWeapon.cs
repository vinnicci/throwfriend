using Godot;
using System;

public class EnemyWeapon : Node2D
{
    [Export] protected PackedScene projectile;
    
    public Enemy ParentNode {get; set;}
    //public Level LevelNode {get; set;}

    protected Position2D spawnPoint;
    protected AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        spawnPoint = (Position2D)GetNode("Sprites/ProjSpawn");
        anim = (AnimationPlayer)GetNode("Anim");
    }


    //ranged
    public virtual void Shoot() {
        anim.Play("shoot");
    }


    //melee
    public virtual void MeleeAttack() {
        anim.Play("melee_attack");
    }


    //used by range attack anim to create projectiles
    public virtual void SpawnProj() {
        EnemyProj proj = (EnemyProj)projectile.Instance();
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, spawnPoint.GlobalRotationDegrees);
    }


    public virtual void FinishShooting() {
        anim.Play("idle");
    }


}
