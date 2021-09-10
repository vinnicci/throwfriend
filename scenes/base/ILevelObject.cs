using System;
using Godot;


public interface ILevelObject
{
    bool Persist {get; set;}
    String SwitchedOnSignal {get; set;}
    String SwitchedOffSignal {get; set;}
    AnimationPlayer TriggerAnim {get; set;}
    Godot.Collections.Array<NodePath> BoundTriggers {get; set;}
    void InitLevelObject();
    void OnTriggeredAllBoundTriggers(NodePath path, bool triggered);
    bool OnSwitchedOn();
    bool OnSwitchedOff();
    void OnAnimFinished(String animName);
}