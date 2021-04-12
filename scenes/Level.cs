using Godot;
using System;

public class Level : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Player playerNode;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerNode = (Player)GetNode("Player");
        playerNode.LevelNode = this;
        foreach(Enemy enemy in GetNode("Enemies").GetChildren()) {
            enemy.LevelNode = this;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
