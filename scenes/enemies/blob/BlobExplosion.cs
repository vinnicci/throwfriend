using Godot;
using System;

public class BlobExplosion : Explosion
{
    public override bool Explode(bool detach = true) {
        if(base.Explode(detach) == false) {
            return false;
        }
        Godot.Collections.Array areas = GetOverlappingAreas();
        foreach(Godot.Object area in areas) {
            if(((Area2D)area).Filename == "res://scenes/triggers/BlobExplosionDetector.tscn") {
                ((ExplosionDetector)area).OnSwitchedOn();
            }
        }
        return true;
    }


}
