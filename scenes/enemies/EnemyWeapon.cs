using Godot;
using System;

public abstract class EnemyWeapon : Node2D
{
    [Export] protected PackedScene projectile;
    
    public Enemy ParentNode {get; set;}

    protected Position2D spawnPoint;
    protected AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        spawnPoint = (Position2D)GetNode("Sprites/ProjSpawn");
        anim = (AnimationPlayer)GetNode("Anim");
    }


    //generic melee action
    public virtual void MeleeAttack() {
        anim.Play("melee_attack");
    }


    //generic ranged action
    public virtual void Shoot() {
        anim.Play("shoot");
    }


    //used by range attack anim to create projectiles
    public virtual void SpawnProj() {
        EnemyProj proj = (EnemyProj)projectile.Instance();
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero, spawnPoint.GlobalRotationDegrees);
    }


    public virtual void FinishShooting() {
        anim.Play("idle");
    }


}
