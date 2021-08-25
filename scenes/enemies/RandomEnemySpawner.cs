using Godot;
using System;

public class RandomEnemySpawner : Position2D
{
    [Export] int maxSpawnCount;
    [Export] EnemySet enemySet;
    [Export] Godot.Collections.Array customSet = new Godot.Collections.Array();
    [Export] Godot.Collections.Array<NodePath> triggers = new Godot.Collections.Array<NodePath>();
    [Export] bool continuous = false;

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


    Godot.Collections.Array currentSpawn = new Godot.Collections.Array();


    void SpawnRandomEnemy() {
        Main mainNode = (Main)GetNode("/root/Main");
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("EnemySpawns");
        Godot.Collections.Array arr = GetEnemySet();
        if(continuous) {
            arr.Shuffle();
            String enemyFilePath = (String)arr[0];
            SpawnEnemy(enemyFilePath);
            return;
        }
        //else if not continuous
        String path = GetPath().ToString();
        if(dict.Contains(path) == false) {
            Godot.Collections.Array enemyArr = new Godot.Collections.Array();
            for(int i = 0; i <= maxSpawnCount - 1; i++) {
                arr.Shuffle();
                String enemyFilePath = (String)arr[0];
                enemyArr.Add(enemyFilePath);
                SpawnEnemy(enemyFilePath);
            }
            dict.Add(path, enemyArr);
        }
        else {
            Godot.Collections.Array enemyArr = (Godot.Collections.Array)dict[path];
            foreach(String enemyFilePath in enemyArr) {
                SpawnEnemy(enemyFilePath);
                //add self as trigger to object
                foreach(NodePath triggerPath in triggers) {
                    Node2D trigger = GetNode<Node2D>(triggerPath);
                    ((ILevelObject)trigger).BoundTriggers.Add(GetPath());
                }
            }
        }
    }


    bool firstSpawn = true;


    void SpawnEnemy(String enemyFilePath) {
        PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyFilePath);
        Enemy enemy = (Enemy)enemyPack.Instance();
        enemy.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        //fade in effect
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
