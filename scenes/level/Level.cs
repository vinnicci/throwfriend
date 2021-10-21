using Godot;
using System;
using System.Collections.Generic;

public abstract class Level : YSort
{
    //by default the Main singleton modifies multiplers
    //if true the Main singleton cannot modify multipliers
    [Export] public bool overrideableEnemyMults;
    [Export] public float enemyHealthMult = 1;
    [Export] public float enemySpeedMult = 1;

    Player playerNode;
    public Player PlayerNode {
        get {
            if(IsInstanceValid(playerNode) && playerNode.Health > 0) {
                return playerNode;
            }
            return default;
        }
        private set {
            playerNode = value;
        }
    }
    Navigation2D nav;
    YSort enemies;
    YSort lvlObjects;
    Main mainNode;
    AnimationPlayer anim;
    Label levelName;


    public override void _Ready()
    {
        base._Ready();
        PlayerEngaging = new Godot.Collections.Array();
        mainNode = (Main)GetNode("/root/Main");
        enemies = (YSort)GetNode("Enemies");
        lvlObjects = (YSort)GetNode("Objects");
        nav = (Navigation2D)GetNode("Nav");
        if(HasNode("Player")) {
            playerNode = (Player)GetNode("Player");
            playerNode.LevelNode = this;
        }
        foreach(Node node in GetChildren()) {
            if(node is Node2D == false) {
                continue;
            }
            else if(node.Name == "Blackboards") {
                foreach(Blackboard blackboard in node.GetChildren()) {
                    blackboard.Init();
                }
            }
            else if(node == enemies) {
                foreach(Enemy enemy in enemies.GetChildren()) {
                    enemy.LevelNode = this;
                }
            }
            else if(node == lvlObjects) {
                foreach(Node2D obj in lvlObjects.GetChildren()) {
                    if(obj is NextLevel) {
                        ((NextLevel)obj).LevelNode = this;
                    }
                    else if(obj is SavePoint) {
                        ((SavePoint)obj).LevelNode = this;
                    }
                    else if(obj is RandomEnemySpawner) {
                        ((RandomEnemySpawner)obj).LevelNode = this;
                    }
                    else if(obj is Trigger) {
                        ((Trigger)obj).LevelNode = this;
                    }
                    else if(obj is LevelTiles) {
                        ((LevelTiles)obj).LevelNode = this;
                    }
                }
            }
        }
        if(IsInstanceValid((Resource)mainNode.Saver.Get("player_save_file")) == false) {
            return;
        }
        //show level name on enter
        Vector2 currentCell = (Vector2)((Resource)mainNode.Saver.Get("player_save_file")).Get("CurrentCell");
        Godot.Collections.Dictionary posDict =
        (Godot.Collections.Dictionary)((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("WorldCells"))[currentCell];
        levelName = (Label)GetNode("CanvasLayer/LevelName");
        levelName.Text = (String)posDict["name"];
        anim = (AnimationPlayer)GetNode("Anim");
        anim.Play("show_level_name");
    }


    public void Spawn(Node2D body, Vector2 pos, float rot = 0) {
        if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            enemies.AddChild(enemy);
            enemy.LevelNode = this;
        }
        else {
            lvlObjects.AddChild((Node2D)body);
        }
        ((Node2D)body).GlobalPosition = pos;
        ((Node2D)body).GlobalRotation = rot;
    }


    //pooling
    // Godot.Collections.Dictionary poolDict = new Godot.Collections.Dictionary();


    // public Node2D GetPooledObj(PackedScene scn) {
    //     Godot.Collections.Array poolArr;
    //     if(poolDict.Contains(scn) == false) {
    //         poolArr = new Godot.Collections.Array();
    //         poolDict.Add(scn, poolArr);
    //     }
    //     else {
    //         poolArr = (Godot.Collections.Array)poolDict[scn];
    //     }
    //     Node2D obj;
    //     if(poolArr.Count > 0) {
    //         obj = (Node2D)poolArr[poolArr.Count - 1];
    //         poolArr.RemoveAt(poolArr.Count - 1);
    //     }
    //     else {
    //         obj = (Node2D)((PackedScene)scn).Instance();
    //     }
    //     GD.Print("pool ya: " + poolArr.Count);
    //     return obj;
    // }


    // public void ReturnObjToPool(Node2D obj) {
    //     PackedScene scn = (PackedScene)ResourceLoader.Load(obj.Filename);
    //     Godot.Collections.Array poolArr = (Godot.Collections.Array)poolDict[scn];
    //     poolArr.Insert(0, obj);
    //     obj.GetParent().RemoveChild(obj);
    //     GD.Print("success return, pool size: " + poolArr.Count);
    // }


    public Godot.Collections.Array PlayerEngaging {get; set;}


    public Vector2 GetPlayerPos() {
        if(playerNode.Health <= 0 || IsInstanceValid(playerNode) == false) {
            return Vector2.Zero;
        }
        return playerNode.GlobalPosition;
    }


    Queue<Line2D> lines = new Queue<Line2D>();


    public Vector2[] GetPath(Vector2 to, Vector2 from) {
        Vector2[] vec;
        vec = nav.GetSimplePath(to, from);
        return vec;
    }


    Queue<Godot.Collections.Array> distCalcQueue = new Queue<Godot.Collections.Array>();


    public void GetDist(Vector2 to, Vector2 from, Node2D aiNode, String key, bool calculatePath) {
        Godot.Collections.Array arrQ = new Godot.Collections.Array();
        arrQ.Add(aiNode); //0
        arrQ.Add(key); //1
        arrQ.Add(to); //2
        arrQ.Add(from); //3
        arrQ.Add(calculatePath); //4
        distCalcQueue.Enqueue(arrQ);
    }


    int CalcDist(Vector2 to, Vector2 from, bool calculatePath) {
        int dist = 0;
        if(calculatePath) {
            Stack<Vector2> arr = new Stack<Vector2>(GetPath(to, from));
            if(arr.Count == 0) {
                dist = -1;
            }
            else {
                dist = (int)arr.Pop().DistanceTo(arr.Peek());
                while(arr.Count > 1) {
                    dist += (int)arr.Pop().DistanceTo(arr.Peek());
                }
            }
        }
        else {
            dist = (int)from.DistanceSquaredTo(to);
        }
        return dist;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //processing distance calculations one frame at a time to reduce frame time?? who knows
        if(distCalcQueue.Count > 0) {
            Godot.Collections.Array arrQ = distCalcQueue.Dequeue();
            if(IsInstanceValid((Node2D)arrQ[0])) {
                ((Node2D)arrQ[0]).Call("update_dist", arrQ[1], CalcDist((Vector2)arrQ[2], (Vector2)arrQ[3], (bool)arrQ[4]));
            }
        }
    }


    //for debugging: show ai path
    // void ShowLine(Vector2[] vec) {
    //     Line2D line = new Line2D();
    //     Tween tween = new Tween();
    //     line.Points = vec;
    //     AddChild(line);
    //     Godot.Collections.Array arr = new Godot.Collections.Array();
    //     arr.Add(line);
    //     tween.InterpolateProperty(line, "modulate", line.Modulate, new Color(1,1,1,0), 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
    //     if(tween.IsConnected("tween_all_completed", this, nameof(OnTweenCompleted)) == false) {
    //         tween.Connect("tween_all_completed", this, nameof(OnTweenCompleted));
    //     }
    //     line.AddChild(tween);
    //     lines.Enqueue(line);
    //     tween.Start();
    // }


    void OnTweenCompleted() {
        lines.Dequeue().QueueFree();
    }


    public void QueueFreeObject(Godot.Object obj) {
        ((Node)obj).QueueFree();
    }


}
