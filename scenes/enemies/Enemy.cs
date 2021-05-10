using Godot;
using System;
using System.Collections.Generic;

public abstract class Enemy : Entity
{
    public Player PlayerNode;
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
    }
        set{
            levelNode = value;
            PlayerNode = levelNode.PlayerNode;
            if(HasNode("EnemyWeapon") == true) {
                WeaponNode = (EnemyWeapon)GetNode("EnemyWeapon");
                WeaponNode.ParentNode = this;
                WeaponNode.LevelNode = LevelNode;
            }
            if(HasNode("AI") == true) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", LevelNode, this);
            }
        }
    }
    public Timer ActionCooldown {get; private set;}
    public EnemyWeapon WeaponNode {get; protected set;}
    protected Explosion ExplosionNode {get; private set;}
    private Node2D aINode;
    protected bool IsActing {get; set;}


    public override void _Ready()
    {
        base._Ready();
        ActionCooldown = (Timer)GetNode("ActionCooldown");
        if(HasNode("Explosion") == true) {
            ExplosionNode = (Explosion)GetNode("Explosion");
        }
        IsActing = false;
    }


    public abstract void OnEnemyBodyEntered(Godot.Object body);


    public virtual bool DoAction(String actionName) {
        if(IsDead == true || IsActing == true || ActionCooldown.IsStopped() == false) {
            return false;
        }
        anim.Play(actionName);
        IsActing = true;
        return true;
    }


    //can't be accessed by animation player unless overridden
    public virtual void FinishAction() {
        IsActing = false;
        ActionCooldown.Start();
        anim.Play("idle");
    }


}
