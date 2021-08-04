using Godot;
using System;

public class RandomEnemySpawner : Position2D
{
    [Export] int maxSpawnCount;
    [Export] EnemySet enemySet;

    public enum EnemySet {
        EASY_SET_1,
        MEDIUM_SET_1,
        HARD_SET_1
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
        (Godot.Collections.Dictionary)((Resource)mainNode.Saver.Get("world_save_file")).Get("EnemySpawns");
        Godot.Collections.Array arr = GetEnemySet();
        //random spawn
        String path = GetPath().ToString();
        if(dict.Contains(path) == false) {
            Godot.Collections.Array enemyArr = new Godot.Collections.Array();
            for(int i = 0; i <= maxSpawnCount - 1; i++) {
                arr.Shuffle();
                String enemyFilePath = (String)arr[0];
                enemyArr.Add(enemyFilePath);
                PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyFilePath);
                Enemy enemy = (Enemy)enemyPack.Instance();
                LevelNode.Spawn(enemy, GlobalPosition);
            }
            dict.Add(path, enemyArr);
        }
        else {
            Godot.Collections.Array enemyArr = (Godot.Collections.Array)dict[path];
            foreach(String enemyPath in enemyArr) {
                PackedScene enemyPack = (PackedScene)ResourceLoader.Load(enemyPath);
                Enemy enemy = (Enemy)enemyPack.Instance();
                LevelNode.Spawn(enemy, GlobalPosition);
            }
        }
    }


    Godot.Collections.Array GetEnemySet() {
        switch(enemySet) {
            case EnemySet.EASY_SET_1:
                return new Godot.Collections.Array(Global.RAND_ENEMY_EASY_SET_1);
            case EnemySet.MEDIUM_SET_1:
                return new Godot.Collections.Array(Global.RAND_ENEMY_MEDIUM_SET_1);
            case EnemySet.HARD_SET_1:
                return new Godot.Collections.Array(Global.RAND_ENEMY_HARD_SET_1);
        }
        return null;
    }


}
