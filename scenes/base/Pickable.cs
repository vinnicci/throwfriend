using Godot;
using System;

public abstract class Pickable : Area2D
{
    protected AnimationPlayer animation;


    public override void _Ready()
    {
        base._Ready();
        animation = (AnimationPlayer)GetNode("Anim");
    }


    public abstract void OnPickableItemBodyEntered(Godot.Object body);


}
