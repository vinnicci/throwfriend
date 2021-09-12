using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public String SwitchedOnSignal {get; set;}
    public String SwitchedOffSignal {get; set;}
    public AnimationPlayer TriggerAnim {get; set;}
    public Level LevelNode {get; set;}

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
        if(TriggerAnim.IsConnected("animation_finished", this, nameof(OnAnimFinished)) == false) {
            TriggerAnim.Connect("animation_finished", this, nameof(OnAnimFinished));
        }
        foreach(NodePath nodePath in BoundTriggers) {
            Node2D node = GetNodeOrNull<Node2D>(nodePath);
            if(IsInstanceValid(node) == false ||
            node.IsConnected("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers))) {
                continue;
            }
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


    public virtual void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        OnSwitchedOn();
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
        if(body is Weapon && ((Weapon)body).CurrentState == Weapon.States.HELD) {
            return;
        }
        OnSwitchedOn();
    }


    public virtual void OnTriggerBodyExited(Godot.Object body) {
        OnSwitchedOff();
    }


    protected bool triggered = false;


    public virtual bool OnSwitchedOn() {
        if(triggered) {
            return false;
        }
        triggered = true;
        TriggerAnim.Queue("trigger");
        EmitSignal(SwitchedOnSignal);
        return true;
    }


    public virtual bool OnSwitchedOff() {
        if(Persist || triggered == false) {
            return false;
        }
        triggered = false;
        TriggerAnim.Queue("trigger_back");
        EmitSignal(SwitchedOffSignal);
        return true;
    }


    public virtual void OnAnimFinished(String animName) {}


}
