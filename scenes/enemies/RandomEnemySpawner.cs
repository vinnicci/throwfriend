using Godot;
using System;

public class RandomEnemySpawner : Position2D
{
    [Export] int maxSpawnCount;
    [Export] EnemySet enemySet;
    [Export] Godot.Collections.Array customSet = new Godot.Collections.Array();

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


    void SpawnRandomEnemy() {
        Main mainNode = (Main)GetNode("/root/Main");
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("EnemySpawns");
        Godot.Collections.Array arr = GetEnemySet();
        String path = GetPath().ToString();
        if(dict.Contains(path) == false) {
            Godot.Collections.Array enemyArr = new Godot.Collections.Array();
            for(int i = 0; i <= maxSpawnCount - 1; i++) {
                arr.Shuffle();
                String enemyFilePath = (String)arr[0];
                enemyArr.Add(enemyFilePath);
                PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyFilePath);
                Enemy enemy = (Enemy)enemyPack.Instance();
                enemy.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
            }
            dict.Add(path, enemyArr);
        }
        else {
            Godot.Collections.Array enemyArr = (Godot.Collections.Array)dict[path];
            foreach(String enemyPath in enemyArr) {
                PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyPath);
                Enemy enemy = (Enemy)enemyPack.Instance();
                enemy.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
            }
        }
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
            default: return customSet;
        }
    }


}
