using Godot;
using System;

public class LaserDetector : Trigger
{
    public override void OnTriggerAreaEntered(Godot.Object area) {
        if(((Area2D)area).Name == "Beam") {
            base.OnTriggerAreaEntered(area);
        }
    }


    public override void OnTriggerAreaExited(Godot.Object area) {
        if(((Area2D)area).Name == "Beam") {
            base.OnTriggerAreaExited(area);
        }
    }


    public override void OnTriggerBodyEntered(Godot.Object body) {}


    public override void OnTriggerBodyExited(Godot.Object body) {}


}
