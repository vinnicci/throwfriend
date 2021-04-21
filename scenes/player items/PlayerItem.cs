using Godot;
using System;

public class PlayerItem : Item
{
    protected Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            InitEffect();
        }
    }


    public override void _Ready()
    {
    }


}
