using Godot;
using System;

public abstract class Enemy : Entity, ISpawner, ILevelObject
{
    [Export] protected Godot.Collections.Dictionary<String, float> acts =
    new Godot.Collections.Dictionary<String, float>();
    [Export] protected Godot.Collections.Array<int> patrolPoints =
    new Godot.Collections.Array<int>();
    [Export] public Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}
    [Export] public bool Persist {get; set;}

    Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set{
            levelNode = value;
            ChangeEntityBaseStats((int)(health*levelNode.enemyHealthMult), (int)(speed*levelNode.enemySpeedMult));
            if(HasNode("EnemyWeapon")) {
                WeaponNode = (EnemyWeapon)GetNode("EnemyWeapon");
                WeaponNode.ParentNode = this;
            }
            if(HasNode("AI")) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", LevelNode, this, patrolPoints);
            }
        }
    }
    public EnemyWeapon WeaponNode {get; protected set;}
    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    
    //not implemented
    public AnimationPlayer TriggerAnim {get; set;}
    public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    Node2D aINode;
    protected Explosion ExplosionNode {get; private set;}    
    protected Godot.Collections.Dictionary ActDict {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        if(HasNode("Explosion")) {
            ExplosionNode = (Explosion)GetNode("Explosion");
        }
        ActDict = new Godot.Collections.Dictionary();
        foreach(String act in acts.Keys) {
            InitAct(act, acts[act]);
        }
        InitLevelObject();
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();



    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        SwitchedOffSignal = nameof(SwitchedOff);
    }


    public void OnSwitchedOn() {
        QueueFree();
    }


    public void OnSwitchedOff() {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnSwitchedOff));
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnTriggeredAllBoundTriggers));
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
        if(body is Weapon && IsInstanceValid(aINode)) {
            Godot.Collections.Dictionary bbDict = (Godot.Collections.Dictionary)aINode.Get("bb");
            Weapon weap = (Weapon)body;
            if(bbDict["enemy"] != weap.PlayerNode) {
                aINode.Call("engage_enemy", weap.PlayerNode);
            }
        }
    }


    public virtual bool DoAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        if(IsDead || (bool)action["IsActive"] || (float)action["TimeRemain"] > 0f) {
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
        if(base.Hit(knockback, damage)) {
            if(IsDead) {
                if(IsInstanceValid(WeaponNode)) {
                    WeaponNode.Disable();
                }
                if(GD.RandRange(0, 100) <= CHANCE_HP_DROP) {
                    CallDeferred(nameof(SpawnInstance), "hp_drop", 1);
                }
                EmitSignal(nameof(SwitchedOn));
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
