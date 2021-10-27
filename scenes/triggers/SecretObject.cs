using Godot;
using System;

public abstract class SecretObject : Trigger
{
    StaticBody2D staticBody;
    uint staticBodyDefaultCollisionLayer;


    public override void _Ready()
    {
        base._Ready();
        staticBody = (StaticBody2D)GetNode("StaticBody2D");
        staticBodyDefaultCollisionLayer = staticBody.CollisionLayer;
    }


    public override bool OnSwitchedOn() {
        if(base.OnSwitchedOn() == false) {
            return false;
        }
        QueueFree();
        return true;
    }


}
