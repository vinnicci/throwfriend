using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
{
    public String SwitchSignal {get; set;}

    AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        SwitchSignal = nameof(Switched);
        anim = (AnimationPlayer)GetNode("Anim");
    }


    [Signal] public delegate void Switched();


    public virtual void OnTriggerAreaEntered(Godot.Object area) {
        anim.Play("trigger");
        EmitSignal(nameof(Switched));
        GD.Print("triggered!");
    }


    public virtual void OnTriggerBodyEntered(Godot.Object body) {
        anim.Play("trigger");
        EmitSignal(nameof(Switched));
        GD.Print("triggered!");
    }


    public void Switch() {
        anim.Play("trigger");
    }


}
