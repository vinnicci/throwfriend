using Godot;
using System;

public abstract class Pickable : Area2D, ISpawnable
{
    protected AnimationPlayer animation;


    public override void _Ready()
    {
        base._Ready();
        animation = (AnimationPlayer)GetNode("Anim");
    }


    public abstract void OnPickableItemBodyEntered(Godot.Object body);


    public void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0, bool homeToPlayer = false) {
        lvl.Spawn(this, globalPos);
    }


}
