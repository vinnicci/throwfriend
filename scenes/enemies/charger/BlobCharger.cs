using Godot;
using System;

public class BlobCharger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "blob") {
            PlaySoundEffect("Spawn");
            for(int i = 0; i <= count - 1; i++) {
                Blob blobInstance = (Blob)spawnScenes[packedSceneKey].Instance();
                blobInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
                blobInstance.ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) *
                (float)GD.RandRange(500, 1000));
                blobInstance.ContinuousCd = RigidBody2D.CCDMode.CastRay;
            }
        }
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }
    
    
}
