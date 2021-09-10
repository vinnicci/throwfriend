using Godot;
using System;

public class SecretTree : Trigger
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
        staticBody.CollisionLayer = 0;
        return true;
    }


    public override bool OnSwitchedOff() {
        if(base.OnSwitchedOff() == false) {
            return false;
        }
        staticBody.CollisionLayer = staticBodyDefaultCollisionLayer;
        return true;
    }


}
