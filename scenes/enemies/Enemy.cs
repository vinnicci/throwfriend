using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Entity
{
    private Node2D aINode;
    private Level levelNode;
    public Player PlayerNode;
    public Level LevelNode {
        get {
            return levelNode;
    }
        set{
            levelNode = value;
            PlayerNode = levelNode.PlayerNode;
            if(HasNode("AI") == true) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", LevelNode, this);
            }
        }
    }


    public virtual void OnEnemyBodyEntered(Godot.Object body) {
    }


    public virtual void DoAction() {
        if(IsDead == true) {
            return;
        }
    }


}
