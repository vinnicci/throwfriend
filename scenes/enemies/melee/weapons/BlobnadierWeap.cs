using Godot;
using System;

public class BlobnadierWeap : EnemyWeapon, IBlobnade
{
    [Export] private PackedScene aiNode;
    private const int KNOCKBACK = 500;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(ParentNode.IsDead == true && anim.IsPlaying() == true) {
            anim.Stop();
            QueueFree();
        }
    }


    private void OnHitboxBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


    public void ThrowBlob() {
        anim.Play("throw_blob");
    }


    public void SpawnBlob() {
        Blob blob = (Blob)projectile.Instance();
        Node2D ai = (Node2D)aiNode.Instance();
        blob.AddChild(ai);
        ai.Call("init_properties", LevelNode, blob);
        Godot.Collections.Dictionary bb = (Godot.Collections.Dictionary)ai.Get("bb");
        Godot.Collections.Array pp = (Godot.Collections.Array)bb["patrol_points"];
        pp.Clear();
        LevelNode.SpawnEnemy(blob, spawnPoint.GlobalPosition);
        blob.ApplyCentralImpulse(new Vector2(2000, 0).Rotated(spawnPoint.GlobalRotation));
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
