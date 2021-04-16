using Godot;
using System;

public class Level : Node2D
{
    public Player PlayerNode {get; private set;}


    public override void _Ready()
    {
        PlayerNode = (Player)GetNode("Player");
        PlayerNode.LevelNode = this;
        foreach(Enemy enemy in GetNode("Enemies").GetChildren()) {
            enemy.LevelNode = this;
        }
    }


}
