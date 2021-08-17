using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
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


    public virtual void OnSwitchedOn() {
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


}
