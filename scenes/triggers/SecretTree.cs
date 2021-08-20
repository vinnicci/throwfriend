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


    public override void OnSwitchedOn() {
        if(Persist && triggered) {
            return;
        }
        triggered = true;
        TriggerAnim.Queue("trigger");
        EmitSignal(SwitchedOnSignal);
        staticBody.CollisionLayer = 0;
    }


    public override void OnSwitchedOff() {
        if(Persist) {
            return;
        }
        triggered = false;
        TriggerAnim.Queue("trigger_back");
        EmitSignal(SwitchedOffSignal);
        staticBody.CollisionLayer = staticBodyDefaultCollisionLayer;
    }


}