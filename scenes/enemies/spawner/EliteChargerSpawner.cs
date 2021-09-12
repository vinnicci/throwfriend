using Godot;
using System;

public class EliteChargerSpawner : BaseChargerSpawner
{
    Polygon2D polyUpper;
    Polygon2D polyLower;


    public override void _Ready()
    {
        base._Ready();
        polyUpper = (Polygon2D)GetNode("Sprite/Upper");
        polyLower = (Polygon2D)GetNode("Sprite/Lower");
    }


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey);
    }


    String[] arr = {
        "charger01", "charger01", "charger02", "charger02", "charger03", "charger03", "charger04"
    };


    [Export] Godot.Collections.Array texSetArr;
    int currentTexSet = 0;


    public void ChangeTextureSet() {
        polyUpper.Texture = (Texture)((Godot.Collections.Array)texSetArr[currentTexSet])[0];
        polyLower.Texture = (Texture)((Godot.Collections.Array)texSetArr[currentTexSet])[1];
        currentTexSet += 1;
        if(currentTexSet > texSetArr.Count - 1) {
            currentTexSet = 0;
        }
    }


    public override void SpawnChargerInstance(String packedSceneKey) {
        Godot.Collections.Array<String> arrStr = new Godot.Collections.Array<String>(arr);
        arrStr.Shuffle();
        packedSceneKey = arrStr[0];
        int count = 1;
        if(packedSceneKey == "charger01") {
            count = 2;
        }
        for(int i = 0; i <= count - 1; i++) {
            ISpawnable bInstance = (ISpawnable)spawnScenes[packedSceneKey].Instance();
            bInstance.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
            ((RigidBody2D)bInstance).ApplyCentralImpulse((LevelNode.GetPlayerPos() - GlobalPosition).Clamped(1) * BLOB_SPAWN_FORCE);
        }
    }
    




}
