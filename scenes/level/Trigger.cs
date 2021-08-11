using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
{
    public String SwitchSignal {get; set;}
    [Export] public bool Persist {get; set;}

    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}


    public override void _Ready()
    {
        base._Ready();
        InitLevelObject();
    }


    public void InitLevelObject() {
        SwitchSignal = nameof(Switched);
        TriggerAnim = (AnimationPlayer)GetNode("Anim");
        foreach(NodePath nodePath in BoundTriggers) {
            Node2D node = GetNodeOrNull<Node2D>(nodePath);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(nodePath);
            node.Connect("Switched", this, nameof(OnTriggeredAllBoundTriggers), arr);
        }
    }


    public void OnTriggeredAllBoundTriggers(NodePath path) {
        BoundTriggers.Remove(path);
        if(BoundTriggers.Count == 0) {
            Switch();
        }
    }


    [Signal] public delegate void Switched();


    public virtual void OnTriggerAreaEntered(Godot.Object area) {
        Switch();
    }


    public virtual void OnTriggerBodyEntered(Godot.Object body) {
        Switch();
    }


    public void Switch() {
        TriggerAnim.Play("trigger");
        EmitSignal(nameof(Switched));
    }


}
