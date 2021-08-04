using Godot;
using System;

public abstract class Wall : TileMap, ILevelObject
{
    [Export] protected bool destructible = false;

    AnimationPlayer anim;
    
    public string SwitchSignal {get; set;}


    public override void _Ready()
    {
        base._Ready();
        anim = (AnimationPlayer)GetNode("Anim");
        SwitchSignal = nameof(Switched);
    }


    //secret walls
    public void FadeOut() {
        EmitSignal(nameof(Switched));
        anim.Play("fade_out");
    }


    [Signal] public delegate void Switched();


    public void Switch() {
        QueueFree();
    }


}
