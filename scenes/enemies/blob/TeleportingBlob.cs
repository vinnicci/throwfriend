using Godot;
using System;

public class TeleportingBlob : Blob
{
    public override void Explode()
    {
        base.Explode();
    }


    public void TeleportToEnemy() {
        Vector2 vec = LevelNode.GetPlayerPos() -
        new Vector2(50,0).Rotated(Mathf.Deg2Rad((float)GD.RandRange(0, 360)));
        Teleport(LevelNode, vec);
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


    public override void ReturnToPool()
    {
        base.ReturnToPool();
    }


}
