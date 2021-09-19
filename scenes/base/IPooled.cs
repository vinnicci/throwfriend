using Godot;
using System;

public interface IPooled
{
    Level LevelNode {get; set;}
    Godot.Collections.Dictionary DefaultVars {get; set;}
    void SetDefaults();
    void ResetDefaults();
    void ReturnToPool();
}
