using System;
using Godot;

public abstract class AllRounderWeapon: EnemyWeapon
{
    //melee
    protected const int KNOCKBACK = 250;


    public virtual void OnHitboxBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


    public void MeleeAttackBack() {
        Anim.Play("melee_attack_back");
    }


    //shoot
    public void ShootBack() {
        Anim.Play("shoot_back");
    }


    //blobnade
    public void ThrowBlob() {
        Anim.Play("throw_blob");
    }


    
    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "blob") {
            for(int i = 0; i <= count; i++) {
                SpawnBlob();
            }
        }
    }


    const int BLOBNADE_THROW_STRENGTH = 1000;


    public void SpawnBlob() {
        Blob blobInstance = (Blob)spawnScenes["blob"].Instance();
        blobInstance.LevelNode = ParentNode.LevelNode;
        blobInstance.Spawn(blobInstance.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero);
        blobInstance.ChangeEntityBaseStats((int)(blobInstance.health*ParentNode.HealthMult),
            (int)(blobInstance.speed*ParentNode.SpeedMult));
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
