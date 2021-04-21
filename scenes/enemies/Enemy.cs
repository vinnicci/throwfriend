using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Entity
{
    private Node2D aINode;
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
    }
        set{
            levelNode = value;
            if(HasNode("AI") == true) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", LevelNode, this);
            }
        }
    }


    public override void _Ready()
    {
        base._Ready();
    }


    public override void _PhysicsProcess(float delta) {
        base._PhysicsProcess(delta);
    }

}
