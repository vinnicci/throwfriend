using Godot;
using System;

public class EnemyProjDetector : Trigger
{
    public override void OnTriggerAreaEntered(Godot.Object area) {
        if(area is EnemyProj) {
            base.OnTriggerAreaEntered(area);
        }
    }


    public override void OnTriggerAreaExited(Godot.Object area) {
        if(area is EnemyProj) {
            base.OnTriggerAreaExited(area);
        }
    }


    public override void OnTriggerBodyEntered(Godot.Object body) {}


    public override void OnTriggerBodyExited(Godot.Object body) {}


}
