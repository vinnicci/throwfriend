using Godot;
using System;

public class Blob : Enemy
{
    public void Explode() {
        if(IsDead) {
            return;
        }
        EmitSignal(nameof(Entity.Died));
        IsDead = true;
        ExplosionNode.Explode();
    }


}
