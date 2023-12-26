using Godot;
using System;

public interface ISpawner
{
    Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}
    void SpawnInstance(String packedSceneKey, int count);
}