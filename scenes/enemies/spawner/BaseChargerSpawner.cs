using Godot;
using System;

public abstract class BaseChargerSpawner : Enemy, ISpawner
{
    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
        if(packedSceneKey == "charger") {
            PlaySoundEffect("Spawn");
            for(int i = 0; i <= count - 1; i++) {
                SpawnChargerInstance(packedSceneKey);
            }
        }
    }


    public virtual void SpawnChargerInstance(String packedSceneKey) {
        ISpawnable bInstance = (ISpawnable)spawnScenes[packedSceneKey].Instance();
        bInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        ((RigidBody2D)bInstance).ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) *
        (float)GD.RandRange(500, 1000));
        ((RigidBody2D)bInstance).ContinuousCd = RigidBody2D.CCDMode.CastRay;
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


}
