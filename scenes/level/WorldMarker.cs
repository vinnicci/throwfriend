using Godot;
using System;

public class WorldMarker : Position2D
{
    [Export] public LevelType levelType;
    public enum LevelType {
        None,
        Start,
        Checkpoint,
        SecretN,
        SecretW,
        SecretE,
        SecretS,
        Secret,
        Misc
    }
    [Export] public PackedScene miscLevelScene;
}
