using Godot;
using System;

public class ExplosionDetector : Trigger
{
    public override void OnTriggerAreaEntered(Godot.Object area) {
        if(area is Explosion) {
            base.OnTriggerAreaEntered(area);
        }
    }


    public override void OnTriggerAreaExited(Godot.Object area) {
        if(area is Explosion) {
            base.OnTriggerAreaExited(area);
        }
    }


    public override void OnTriggerBodyEntered(Godot.Object body) {}


    public override void OnTriggerBodyExited(Godot.Object body) {}


}
