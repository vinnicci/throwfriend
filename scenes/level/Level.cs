using Godot;
using System;
using System.Collections.Generic;

public abstract class Level : YSort
{
    [Export] public bool overrideableEnemyMults = true;
    [Export] public float enemyHealthMult = 1;
    [Export] public float enemySpeedMult = 1;

    Player playerNode;
    Navigation2D nav;
    YSort enemies;
    YSort lvlObjects;
    Main mainNode;


    public override void _Ready()
    {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        enemies = (YSort)GetNode("Enemies");
        lvlObjects = (YSort)GetNode("Objects");
        nav = (Navigation2D)GetNode("Nav");
        if(HasNode("Player")) {
            playerNode = (Player)GetNode("Player");
            playerNode.LevelNode = this;
        }
        foreach(Node2D node in GetChildren()) {
            if(node.Name == "Blackboards") {
                foreach(Blackboard blackboard in node.GetChildren()) {
                    blackboard.Init();
                }
            }
            else if(node == enemies) {
                foreach(Enemy enemy in enemies.GetChildren()) {
                    enemy.LevelNode = this;
                    enemy.Connect(nameof(Entity.Died), this, nameof(OnEnemyDead));
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
                        ((RandomEnemySpawner)node).LevelNode = this;
                    }
                }
            }
        }
    }


    public int PlayerEngaging {get; set;}


    void OnEnemyDead() {
        PlayerEngaging -= 1;
        GD.Print("died: " + PlayerEngaging);
    }


    public void Spawn(ISpawnable body, Vector2 pos, float rot = 0) {
        if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            enemy.LevelNode = this;
            enemy.Connect(nameof(Entity.Died), this, nameof(OnEnemyDead));
            enemies.AddChild(enemy);
        }
        else {
            AddChild((Node2D)body);
        }
        ((Node2D)body).GlobalPosition = pos;
        ((Node2D)body).GlobalRotation = rot;
    }


    public Vector2 GetPlayerPos() {
        if(playerNode.IsDead || IsInstanceValid(playerNode) == false) {
            return Vector2.Zero;
        }
        return playerNode.GlobalPosition;
    }


    Queue<Line2D> lines = new Queue<Line2D>();


    public Vector2[] GetPath(Vector2 to, Vector2 from) {
        Vector2[] vec;
        vec = nav.GetSimplePath(to, from);
        //ShowLine(vec);
        return vec;
    }


    public int GetDist(Vector2 to, Vector2 from) {
        Stack<Vector2> arr = new Stack<Vector2>(GetPath(to, from));
        if(arr.Count == 0) {
            return -1;
        }
        int dist = (int)arr.Pop().DistanceTo(arr.Peek());
        while(arr.Count > 1) {
            dist += (int)arr.Pop().DistanceTo(arr.Peek());
        }
        return dist;
    }


    //debug: show ai path
    void ShowLine(Vector2[] vec) {
        Line2D line = new Line2D();
        Tween tween = new Tween();
        line.Points = vec;
        AddChild(line);
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(line);
        tween.InterpolateProperty(line, "modulate", line.Modulate, new Color(1,1,1,0), 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        if(tween.IsConnected("tween_all_completed", this, nameof(OnTweenCompleted)) == false) {
            tween.Connect("tween_all_completed", this, nameof(OnTweenCompleted));
        }
        line.AddChild(tween);
        lines.Enqueue(line);
        tween.Start();
    }


    void OnTweenCompleted() {
        lines.Dequeue().QueueFree();
    }


}
