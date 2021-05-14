using Godot;
using System;
using System.Collections.Generic;

public abstract class Enemy : Entity
{
    [Export] protected Godot.Collections.Array<float> actCooldown;

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
    public EnemyWeapon WeaponNode {get; protected set;}
    protected Explosion ExplosionNode {get; private set;}
    private Node2D aINode;
    protected Godot.Collections.Dictionary ActDict {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        if(HasNode("Explosion") == true) {
            ExplosionNode = (Explosion)GetNode("Explosion");
        }
        ActDict = new Godot.Collections.Dictionary();
    }


    protected void InitAct(String actionName, float length) {
        Godot.Collections.Dictionary act = new Godot.Collections.Dictionary();
        act["IsActive"] = false;
        act["TimeRemain"] = 0f;
        act["Length"] = length;
        ActDict[actionName] = act;
    }


    public bool IsActActive(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            String err = "Action " + actionName + " does not exist.";
            GD.PrintErr(err);
            throw new Exception(err);
        }
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        return (bool)action["IsActive"];
    }


    public bool IsActCoolingDown(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            String err = "Action " + actionName + " does not exist.";
            GD.PrintErr(err);
            throw new Exception(err);
        }
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        return (float)action["TimeRemain"] > 0;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        foreach(String actionName in ActDict.Keys) {
            Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
            float timeRemain = (float)action["TimeRemain"];
            if(timeRemain > 0f) {
                timeRemain -= delta;
                action["TimeRemain"] = timeRemain;
            }
        }
    }


    public virtual void OnEnemyBodyEntered(Godot.Object body) {
        if(body is Weapon && IsInstanceValid(aINode) == true) {
            aINode.Call("engage_enemy", LevelNode.PlayerNode);
        }
    }


    public virtual bool DoAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        if(IsDead == true || (bool)action["IsActive"] == true || (float)action["TimeRemain"] > 0f) {
            return false;
        }
        action["IsActive"] = true;
        anim.Play(actionName);
        return true;
    }


    //can't be accessed by animation player unless overridden
    public virtual void FinishAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        action["IsActive"] = false;
        action["TimeRemain"] = action["Length"];
        anim.Play("idle");
    }


}
