using Godot;
using System;

public class Props : TileMap, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}
    
    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}

    uint defaultCollisionLayer;
    uint defaultCollisionMask;


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
        if(Persist && BoundTriggers.Count == 0) {
            return;
        }
        if(triggered) {
            BoundTriggers.Remove(path);
        }
        else {
            if(BoundTriggers.Count == 0) {
                OnSwitchedOff();
            }
            BoundTriggers.Add(path);            
        }
        if(BoundTriggers.Count == 0) {
            OnSwitchedOn();
        }
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();


    public void OnSwitchedOn() {
        TriggerAnim.Queue("trigger");
        EmitSignal(SwitchedOnSignal);
    }


    public virtual void OnSwitchedOff() {
        if(Persist) {
            return;
        }
        TriggerAnim.Queue("trigger_back");
        EmitSignal(SwitchedOffSignal);
    }


    public virtual void OnAnimFinished(String animName) {
        if(animName == "trigger") {
            CollisionLayer = 0;
            CollisionMask = 0;
        }
        else if(animName == "trigger_back") {
            CollisionLayer = defaultCollisionLayer;
            CollisionMask = defaultCollisionMask;
        }
    }


}
