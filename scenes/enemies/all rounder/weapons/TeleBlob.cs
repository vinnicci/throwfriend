using Godot;
using System;

public class TeleBlob : EnemyProj
{
    Vector2 teleDest;


    public override void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRotDeg = 0) {
        base.Spawn(lvl, globalPos, destination);
        teleDest = GlobalPosition;
    }


    public override bool OnEnemyProjBodyEntered(Godot.Object body) {
        if(base.OnEnemyProjBodyEntered(body) == false) {
            return false;
        }
        Player player = (Player)body;
        player.Teleport(levelNode, teleDest);
        return true;        
    }


}
