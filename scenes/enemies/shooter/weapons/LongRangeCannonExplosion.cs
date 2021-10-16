using Godot;
using System;

public class LongRangeCannonExplosion : Explosion
{
    public override bool Explode(bool detachSoundNode = true) {
        if(base.Explode(detachSoundNode) == false) {
            return false;
        }
        Godot.Collections.Array areas = GetOverlappingAreas();
        foreach(Godot.Object area in areas) {
            if(((Area2D)area).Filename == "res://scenes/triggers/CannonProjExplosionDetector.tscn") {
                ((ExplosionDetector)area).OnSwitchedOn();
            }
        }
        return true;
    }


}
