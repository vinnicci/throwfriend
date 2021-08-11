using Godot;
using System;

public abstract class Collectable : Area2D, ISpawnable, ILevelObject
{
    [Export] public bool Persist {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}

    public AnimationPlayer TriggerAnim {get; set;}
    public String SwitchSignal {get; set;}


    public override void _Ready()
    {
        base._Ready();
        InitLevelObject();
    }


    public void InitLevelObject() {
        SwitchSignal = nameof(Switched);
        TriggerAnim = (AnimationPlayer)GetNode("Anim");
    }


    public void OnTriggeredAllBoundTriggers(NodePath path) {
        GD.PrintErr("Collectable doesn't implement bound trigger functions.");
    }


    [Signal] public delegate void Switched();


    public virtual void OnCollectableBodyEntered(Godot.Object body) {
        EmitSignal(nameof(Switched));
    }


    public void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0) {
        lvl.Spawn(this, globalPos);
    }


    public void Switch() {
        QueueFree();
    }


}
