using Godot;
using System;

public abstract class Trigger : Area2D, ILevelObject
{
    public String SwitchSignal {get; set;}
    [Export] public bool Persist {get; set;}

    [Export] Godot.Collections.Array<NodePath> actObjects = new Godot.Collections.Array<NodePath>();
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
        ActivateObjects();
    }


    public virtual void OnTriggerBodyEntered(Godot.Object body) {
        anim.Play("trigger");
        EmitSignal(nameof(Switched));
        ActivateObjects();
    }


    void ActivateObjects() {
        foreach(NodePath path in actObjects) {
            Wall wall = GetNodeOrNull<Wall>(path);
            if(IsInstanceValid(wall)) {
                wall.FadeOut();
            }
        }
    }


    public void Switch() {
        anim.Play("trigger");
    }


}
