using Godot;
using System;

public class BlobnadeAndBlaster : BaseAllRounderWeapon
{
    [Export] PackedScene projectile2;

    const int BLOBNADE_THROW_STRENGTH = 1000;


    public override void SpawnBlob()
    {
        Blob blob = (Blob)projectile2.Instance();
        Node2D ai = (Node2D)aiNode.Instance();
        blob.AddChild(ai);
        ai.Call("init_properties", ParentNode.LevelNode, blob);
        blob.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition);
        blob.ApplyCentralImpulse(new Vector2(BLOBNADE_THROW_STRENGTH, 0).Rotated(spawnPoint.GlobalRotation));
    }


    public override void SpawnProj() {
        base.SpawnProj();
    }


    public override void FinishShooting() {
        base.FinishShooting();
    }

    
}
