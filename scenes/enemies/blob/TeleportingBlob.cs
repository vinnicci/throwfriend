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
        new Vector2(50,0).Rotated(Godot.Mathf.Deg2Rad((float)GD.RandRange(0, 360)));
        Teleport(LevelNode, vec);
    }


}
