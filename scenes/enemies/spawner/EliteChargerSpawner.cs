using Godot;
using System;

public class EliteChargerSpawner : BaseChargerSpawner
{
    Polygon2D polyUpper;
    Polygon2D polyLower;


    public override void _Ready()
    {
        base._Ready();
        polyUpper = (Polygon2D)GetNode("Sprite/Lower/Upper");
        polyLower = (Polygon2D)GetNode("Sprite/Lower");
    }


    String[] arr = {
        "charger01", "charger01", "charger02", "charger02", "charger03", "charger03", "charger04"
    };


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        if(packedSceneKey == "charger") {
            for(int i = 0; i <= count-1; i++) {
                Godot.Collections.Array<String> arrStr = new Godot.Collections.Array<String>(arr);
                arrStr.Shuffle();
                packedSceneKey = arrStr[0];
                SpawnChargerInstance(packedSceneKey);
            }
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
            base.SpawnChargerInstance(packedSceneKey);
        }
    }


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


}
