using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{
    public Player PlayerNode {get; private set;}

    private Navigation2D nav;


    public override void _Ready()
    {
        PlayerNode = (Player)GetNode("Player");
        PlayerNode.LevelNode = this;
        foreach(Enemy enemy in GetNode("Enemies").GetChildren()) {
            enemy.LevelNode = this;
        }
        nav = (Navigation2D)GetNode("Nav");
    }


    Queue<Line2D> lines = new Queue<Line2D>();


    public Vector2[] GetPath(Vector2 to, Vector2 from) {
        Vector2[] vec;
        vec = nav.GetSimplePath(to, from);
        // Line2D line = new Line2D();
        // Tween tween = new Tween();
        // line.Points = vec;
        // AddChild(line);
        // Godot.Collections.Array arr = new Godot.Collections.Array();
        // arr.Add(line);
        // tween.InterpolateProperty(line, "modulate", line.Modulate, new Color(1,1,1,0), 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        // tween.Connect("tween_all_completed", this, "OnTweenCompleted");
        // line.AddChild(tween);
        // lines.Enqueue(line);
        // tween.Start();
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


    private void OnTweenCompleted() {
        lines.Dequeue().QueueFree();
    }


}
