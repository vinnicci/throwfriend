using Godot;
using System;

public class AcidDetector : Trigger
{
    public override void OnTriggerAreaEntered(Godot.Object area) {
        if(((Area2D)area).Filename == "res://scenes/enemies/shooter/weapons/SprayerAcidProj.tscn") {
            base.OnTriggerAreaEntered(area);
        }
    }


    public override void OnTriggerAreaExited(Godot.Object area) {
        if(((Area2D)area).Filename == "res://scenes/enemies/shooter/weapons/SprayerAcidProj.tscn") {
            base.OnTriggerAreaExited(area);
        }
    }


    public override void OnTriggerBodyEntered(Godot.Object body) {}


    public override void OnTriggerBodyExited(Godot.Object body) {}


}
