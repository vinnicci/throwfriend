using Godot;
using System;

public class EnemyWeapon : Node2D
{
    [Export] protected PackedScene projectile;
    
    public Enemy ParentNode {get; set;}
    public Level LevelNode {get; set;}

    private Position2D spawnPoint;
    private AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        spawnPoint = (Position2D)GetNode("ProjSpawn");
        anim = (AnimationPlayer)GetNode("Anim");
    }


    public virtual void Shoot() {
        anim.Play("shoot");
    }


    public virtual void SpawnProj() {
        EnemyProj proj = (EnemyProj)projectile.Instance();
        proj.Spawn(spawnPoint.GlobalPosition, GlobalRotationDegrees, LevelNode);
    }


    public virtual void FinishShooting() {
        anim.Play("idle");
    }


}