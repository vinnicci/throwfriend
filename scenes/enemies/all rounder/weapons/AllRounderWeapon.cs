using System;
using Godot;

public abstract class AllRounderWeapon: EnemyWeapon
{
    [Export] private PackedScene aiNode; //used by blobnades


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(ParentNode.IsDead == true && anim.IsPlaying() == true) {
            anim.Stop();
            QueueFree();
        }
    }


    //melee
    protected const int KNOCKBACK = 500;


    public virtual void OnHitboxBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


    //blobnade
    public void ThrowBlob() {
        anim.Play("throw_blob");
    }


    public void SpawnBlob() {
        Blob blob = (Blob)projectile.Instance();
        Node2D ai = (Node2D)aiNode.Instance();
        blob.AddChild(ai);
        ai.Call("init_properties", ParentNode.LevelNode, blob);
        blob.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition);
        blob.ApplyCentralImpulse(new Vector2(2000, 0).Rotated(spawnPoint.GlobalRotation));
    }


    //ranged
    public override void SpawnProj()
    {
        GlobalRotationDegrees -= 30;
        base.SpawnProj();
        GlobalRotationDegrees += 15;
        base.SpawnProj();
        GlobalRotationDegrees += 15;
        base.SpawnProj();
        GlobalRotationDegrees += 15;
        base.SpawnProj();
        GlobalRotationDegrees += 15;
        base.SpawnProj();
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
