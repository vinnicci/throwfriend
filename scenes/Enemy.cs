using Godot;
using System;

public class Enemy : Entity
{
    public Level LevelNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
    }


    public override void _PhysicsProcess(float delta) {
        base._PhysicsProcess(delta);
    }
    

}
