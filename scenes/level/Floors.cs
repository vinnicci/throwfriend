using Godot;
using System;

public class Floors : TileMap, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}


    public override void _Ready()
    {
        base._Ready();
        InitLevelObject();
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
        if(triggered) {
            BoundTriggers.Remove(path);
        }
        else {
            BoundTriggers.Add(path);
        }
        if(BoundTriggers.Count == 0) {
            OnSwitchedOn();
        }
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();


    public void OnSwitchedOn() {
        TriggerAnim.Play("trigger");
        EmitSignal(SwitchedOnSignal);
    }


    public void OnSwitchedOff() {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnSwitchedOff));
    }
    
    
}
