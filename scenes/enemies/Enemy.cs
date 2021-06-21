using Godot;
using System;

public abstract class Enemy : Entity, ISpawner
{
    [Export] protected Godot.Collections.Dictionary<String, float> acts =
    new Godot.Collections.Dictionary<String, float>();
    [Export] protected Godot.Collections.Array<int> patrolPoints =
    new Godot.Collections.Array<int>();
    [Export] public Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}

    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set{
            levelNode = value;
            if(HasNode("EnemyWeapon") == true) {
                WeaponNode = (EnemyWeapon)GetNode("EnemyWeapon");
                WeaponNode.ParentNode = this;
            }
            if(HasNode("AI") == true) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", LevelNode, this, patrolPoints);
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
        foreach(String act in acts.Keys) {
            InitAct(act, acts[act]);
        }
    }


    protected void InitAct(String actionName, float length) {
        Godot.Collections.Dictionary act = new Godot.Collections.Dictionary();
        act["IsActive"] = false;
        act["TimeRemain"] = 0f;
        act["Length"] = length;
        ActDict[actionName] = act;
    }


    public bool HasAct(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            return false;
        }
        return true;
    }


    public bool IsActActive(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            return false;
        }
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        return (bool)action["IsActive"];
    }


    public bool IsActCoolingDown(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            return false;
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
            Weapon weap = (Weapon)body;
            aINode.Call("engage_enemy", weap.PlayerNode);
        }
    }


    public virtual bool DoAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        if(IsDead == true || (bool)action["IsActive"] == true || (float)action["TimeRemain"] > 0f) {
            return false;
        }
        action["IsActive"] = true;
        anim.Stop();
        anim.Play(actionName);
        return true;
    }


    const float CHANCE_HP_DROP = 10f;


    public override bool Hit(Vector2 knockback, int damage)
    {
        if(base.Hit(knockback, damage) == true) {
            if(IsDead == true && GD.RandRange(0, 100) <= CHANCE_HP_DROP) {
                CallDeferred("SpawnInstance", "hp_drop", 1);
            }
            return true;
        }
        return false;
    }


    public virtual void SpawnInstance(String packedSceneKey, int count = 1) {
        if(packedSceneKey == "hp_drop") {
            HealthPickup healthInst = (HealthPickup)spawnScenes[packedSceneKey].Instance();
            healthInst.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        }
    }


    //override and use this func to end action anims
    public virtual void FinishAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        action["IsActive"] = false;
        action["TimeRemain"] = action["Length"];
        anim.Play("idle");
    }


}
