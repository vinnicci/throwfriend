using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}

    protected uint defaultCollisionLayer;
    protected uint defaultCollisionMask;


    public override void _Ready()
    {
        base._Ready();
        InitLevelObject();
        defaultCollisionLayer = CollisionLayer;
        defaultCollisionMask = CollisionMask;
    }


    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        SwitchedOffSignal = nameof(SwitchedOff);
        TriggerAnim = (AnimationPlayer)GetNode("Anim");
        foreach(NodePath nodePath in BoundTriggers) {
            Node2D node = GetNodeOrNull<Node2D>(nodePath);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(nodePath);
            arr.Add(true);
            node.Connect("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers), arr);
            arr = new Godot.Collections.Array();
            arr.Add(nodePath);
            arr.Add(false);
            node.Connect("SwitchedOff", this, nameof(OnTriggeredAllBoundTriggers), arr);
        }
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnTriggeredAllBoundTriggers));
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();


    public virtual void OnTriggerAreaEntered(Godot.Object area) {
        OnSwitchedOn();
    }


    public virtual void OnTriggerAreaExited(Godot.Object area) {
        OnSwitchedOff();
    }


    public virtual void OnTriggerBodyEntered(Godot.Object body) {
        OnSwitchedOn();
    }


    public virtual void OnTriggerBodyExited(Godot.Object body) {
        OnSwitchedOff();
    }


    protected bool triggered = false;


    public virtual void OnSwitchedOn() {
        if(Persist && triggered) {
            return;
        }
        triggered = true;
        TriggerAnim.Queue("trigger");
        EmitSignal(SwitchedOnSignal);
    }


    public virtual void OnSwitchedOff() {
        if(Persist) {
            return;
        }
        triggered = false;
        TriggerAnim.Queue("trigger_back");
        EmitSignal(SwitchedOffSignal);
    }


}
