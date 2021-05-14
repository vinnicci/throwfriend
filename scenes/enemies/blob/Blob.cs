using Godot;
using System;

public class Blob : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("explode", (float)actCooldown[0]);
    }


    public void Explode() {
        ExplosionNode.Explode();
    }


}
