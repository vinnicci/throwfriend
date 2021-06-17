using System;
using Godot;

public abstract class BaseAllRounderWeapon: EnemyWeapon
{
    [Export] protected PackedScene aiNode; //used by blobnades


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(ParentNode.IsDead == true && anim.IsPlaying() == true) {
            anim.Stop();
            QueueFree();
        }
    }


    //melee
    protected const int KNOCKBACK = 250;


    public virtual void OnHitboxBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


    public void MeleeAttackBack() {
        anim.Play("melee_attack_back");
    }


    //shoot
    public void ShootBack() {
        anim.Play("shoot_back");
    }


    //blobnade
    public void ThrowBlob() {
        anim.Play("throw_blob");
    }


    const int BLOBNADE_THROW_STRENGTH = 1000;


    public virtual void SpawnBlob() {
        Blob blobInstance = (Blob)projectile.Instance();
        Node2D ai = (Node2D)aiNode.Instance();
        blobInstance.AddChild(ai);
        blobInstance.LevelNode = ParentNode.LevelNode;
        blobInstance.Spawn(blobInstance.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero);
        blobInstance.ApplyCentralImpulse(new Vector2(BLOBNADE_THROW_STRENGTH, 0).Rotated(spawnPoint.GlobalRotation));
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
