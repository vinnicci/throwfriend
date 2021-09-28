using Godot;
using System;

public class RandomEnemySpawner : Position2D, ILevelObject
{
    [Export] int maxSpawnCount;
    [Export] EnemySet enemySet;
    [Export] Godot.Collections.Array customSet = new Godot.Collections.Array();
    [Export] Godot.Collections.Array<NodePath> triggers = new Godot.Collections.Array<NodePath>();
    [Export] bool continuous = false;
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public String SwitchedOnSignal {get; set;}
    
    //not implemented
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}
    public bool Persist {get; set;}

    public enum EnemySet {
        EASY_ALL_ROUNDERS,
        MEDIUM_ALL_ROUNDERS,
        HARD_ALL_ROUNDERS,
        EASY_CHARGERS,
        MEDIUM_CHARGERS,
        HARD_CHARGERS,
        EASY_SHOOTERS,
        MEDIUM_SHOOTERS,
        HARD_SHOOTERS,
        EASY_RANDOM,
        MEDIUM_RANDOM,
        HARD_RANDOM,
        ELITE_ALL_ROUNDERS,
        ELITE_CHARGERS,
        ELITE_SHOOTERS,
        CUSTOM
    }
    Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set {
            levelNode = value;
            if(continuous == false) {
                InitLevelObject();
            }
            SpawnRandomEnemy();
        }
    }


    Timer spawnTimer;
    Tween tween;


    public override void _Ready()
    {
        base._Ready();
        spawnTimer = (Timer)GetNode("SpawnTimer");
        tween = (Tween)GetNode("Tween");
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();


    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        foreach(NodePath nodePath in BoundTriggers) {
            Node2D node = GetNodeOrNull<Node2D>(nodePath);
            if(IsInstanceValid(node) == false ||
            node.IsConnected("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers))) {
                continue;
            }
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(nodePath);
            arr.Add(true);
            node.Connect("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers), arr);
        }
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        if(triggered) {
            BoundTriggers.Remove(path);
        }
        if(BoundTriggers.Count == 0) {
            OnSwitchedOn();
        }
    }


    public virtual bool OnSwitchedOn() {
        QueueFree();
        return true;
    }


    public virtual bool OnSwitchedOff() {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnSwitchedOff));
        return false;
    }


    public void OnAnimFinished(String animName) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnAnimFinished));
    }


    Godot.Collections.Array currentSpawn = new Godot.Collections.Array();


    void SpawnRandomEnemy() {
        Godot.Collections.Array arr = GetEnemySet();
        Godot.Collections.Array enemyArr = new Godot.Collections.Array();
        for(int i = 0; i <= maxSpawnCount - 1; i++) {
            arr.Shuffle();
            String enemyFilePath = (String)arr[0];
            enemyArr.Add(enemyFilePath);
        }
        if(continuous) {
            foreach(String enemyFilePath in enemyArr) {
                SpawnEnemy(enemyFilePath);
            }
            return;
        }
        //else if not continuous
        Main mainNode = (Main)GetNode("/root/Main");
        if(IsInstanceValid(mainNode.WorldSaveFile) == false) {
            foreach(String enemyFilePath in enemyArr) {
                SpawnEnemy(enemyFilePath);
            }
            return;
        }
        //if savefile valide
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("EnemySpawns");
        String path = mainNode.PlayerSaveFile.Get("CurrentCell") + Name;
        if(dict.Contains(path) == false) {
            foreach(String enemyFilePath in enemyArr) {
                SpawnEnemy(enemyFilePath);
            }
            dict.Add(path, enemyArr);
        }
        else {
            enemyArr = (Godot.Collections.Array)dict[path];
            foreach(String enemyFilePath in enemyArr) {
                SpawnEnemy(enemyFilePath);
            }
        }
    }


    bool firstSpawn = true;


    void SpawnEnemy(String enemyFilePath) {
        PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyFilePath);
        Enemy enemy = (Enemy)enemyPack.Instance();
        enemy.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        //make enemy node as one of bound triggers
        foreach(NodePath triggerPath in triggers) {
            Node2D trigger = GetNodeOrNull<Node2D>(triggerPath);
            if(((ILevelObject)trigger).BoundTriggers.Contains(enemy.GetPath()) == false) {
                ((ILevelObject)trigger).BoundTriggers.Add(enemy.GetPath());
            }
            ((ILevelObject)trigger).InitLevelObject();
        }
        //fade in effect
        enemy.Modulate = new Color(1,1,1,0);
        tween.InterpolateProperty(enemy, "modulate", new Color(1,1,1,0), new Color(1,1,1,1),
        0.3f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
        //enemy will engage player immediately if continuous
        if(firstSpawn == false && continuous && enemy.HasNode("AI")) {
            enemy.GetNode("AI").Call("engage_enemy", LevelNode.PlayerNode);
        }
        else if(firstSpawn) {
            firstSpawn = false;
        }
        currentSpawn.Add(enemy);
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(enemy);
        enemy.Connect(nameof(Enemy.Died), this, nameof(OnEnemyDied), arr);
    }


    void OnEnemyDied(Enemy enemy) {
        currentSpawn.Remove(enemy);
        if(continuous && currentSpawn.Count == 0) {
            spawnTimer.Start();
        }
    }


    void OnSpawnTimerTimeout() {
        Godot.Collections.Array arr = GetEnemySet();
        arr.Shuffle();
        String enemyFilePath = (String)arr[0];
        SpawnEnemy(enemyFilePath);
    }


    Godot.Collections.Array GetEnemySet() {
        switch(enemySet) {
            case EnemySet.EASY_ALL_ROUNDERS: return new Godot.Collections.Array(Global.EASY_ALL_ROUNDERS);
            case EnemySet.MEDIUM_ALL_ROUNDERS: return new Godot.Collections.Array(Global.MEDIUM_ALL_ROUNDERS);
            case EnemySet.HARD_ALL_ROUNDERS: return new Godot.Collections.Array(Global.HARD_ALL_ROUNDERS);
            case EnemySet.EASY_CHARGERS: return new Godot.Collections.Array(Global.EASY_CHARGERS);
            case EnemySet.MEDIUM_CHARGERS: return new Godot.Collections.Array(Global.MEDIUM_CHARGERS);
            case EnemySet.HARD_CHARGERS: return new Godot.Collections.Array(Global.HARD_CHARGERS);
            case EnemySet.EASY_SHOOTERS: return new Godot.Collections.Array(Global.EASY_SHOOTERS);
            case EnemySet.MEDIUM_SHOOTERS: return new Godot.Collections.Array(Global.MEDIUM_SHOOTERS);
            case EnemySet.HARD_SHOOTERS: return new Godot.Collections.Array(Global.HARD_SHOOTERS);
            case EnemySet.EASY_RANDOM: return new Godot.Collections.Array(Global.EASY_RANDOM);
            case EnemySet.MEDIUM_RANDOM: return new Godot.Collections.Array(Global.MEDIUM_RANDOM);
            case EnemySet.HARD_RANDOM: return new Godot.Collections.Array(Global.HARD_RANDOM);
            case EnemySet.ELITE_ALL_ROUNDERS: return new Godot.Collections.Array(Global.ELITE_ALL_ROUNDERS);
            case EnemySet.ELITE_CHARGERS: return new Godot.Collections.Array(Global.ELITE_CHARGERS);
            case EnemySet.ELITE_SHOOTERS: return new Godot.Collections.Array(Global.ELITE_SHOOTERS);
            default: return customSet;
        }
    }


}
