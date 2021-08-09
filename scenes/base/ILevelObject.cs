using System;
using Godot;


public interface ILevelObject
{
    bool Persist {get; set;}
    String SwitchSignal {get; set;}
    void Switch();
}