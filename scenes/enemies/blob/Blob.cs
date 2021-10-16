using Godot;
using System;

public class Blob : Enemy
{
    public virtual void Explode() {
        if(Health <= 0) {
            return;
        }
        LevelNode.PlayerEngaging.Remove(Name);
        EmitSignal(nameof(Died));
        Health = 0;
        ExplosionNode.Explode();
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


    public virtual void ReturnToPool() {
        QueueFree();
    }


}
