using Godot;
using System;

public class LaserDetector : Trigger
{
    Particles2D particles;


    public override void _Ready()
    {
        base._Ready();
        particles = (Particles2D)GetNode("Particles2D");
    }


    public override void OnTriggerAreaEntered(Godot.Object area) {
        if(((Area2D)area).Name == "Beam") {
            base.OnTriggerAreaEntered(area);
            particles.Emitting = true;
        }
    }


    public override void OnTriggerAreaExited(Godot.Object area) {
        if(((Area2D)area).Name == "Beam") {
            base.OnTriggerAreaExited(area);
            particles.Emitting = false;
        }
    }


    public override void OnTriggerBodyEntered(Godot.Object body) {}


    public override void OnTriggerBodyExited(Godot.Object body) {}


}
