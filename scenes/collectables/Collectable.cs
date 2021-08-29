using Godot;
using System;

public abstract class Collectable : Area2D, ISpawnable, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public AnimationPlayer TriggerAnim {get; set;}
    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}


    public override void _Ready()
    {
        base._Ready();
        InitLevelObject();
    }


    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        TriggerAnim = (AnimationPlayer)GetNode("Anim");
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnTriggeredAllBoundTriggers));
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();


    public virtual void OnCollectableBodyEntered(Godot.Object body) {
        EmitSignal(SwitchedOnSignal);
    }


    public void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0) {
        lvl.Spawn(this, globalPos);
    }


    public void OnSwitchedOn() {
        QueueFree();
    }


    public void OnSwitchedOff() {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnSwitchedOff));
    }


    public void OnAnimFinished(String animName) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnAnimFinished));
    }


}
