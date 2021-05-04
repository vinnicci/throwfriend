using Godot;
using System;

public class Pickable : Area2D
{
    protected AnimationPlayer animation;


    public override void _Ready()
    {
        base._Ready();
        animation = (AnimationPlayer)GetNode("Anim");
    }


    public virtual void OnPickableItemBodyEntered(Godot.Object body) {
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
    }


    private void OnAnimAnimationFinished(String animName) {
        QueueFree();
    }
}
