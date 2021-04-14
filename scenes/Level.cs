using Godot;
using System;

public class Level : Node2D
{
    private Player playerNode;


    public override void _Ready()
    {
        playerNode = (Player)GetNode("Player");
        playerNode.LevelNode = this;
        foreach(Enemy enemy in GetNode("Enemies").GetChildren()) {
            enemy.LevelNode = this;
        }
    }


}
