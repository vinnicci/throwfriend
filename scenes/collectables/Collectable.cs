using Godot;
using System;

public abstract class Collectable : Area2D, ISpawnable, ILevelObject
{
    public String SwitchSignal {get; set;}
    [Export] public bool Persist {get; set;}

    protected AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        SwitchSignal = nameof(Switched);
        anim = (AnimationPlayer)GetNode("Anim");
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
