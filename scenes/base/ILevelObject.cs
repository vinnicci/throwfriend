using System;
using Godot;


public interface ILevelObject
{
    String SwitchSignal {get; set;}
    void Switch();
}