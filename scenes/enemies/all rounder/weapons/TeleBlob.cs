using Godot;
using System;

public class TeleBlob : EnemyProj
{
    Vector2 teleDest;


    public override void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRotDeg = 0) {
        base.Spawn(lvl, globalPos, destination);
        teleDest = GlobalPosition;
    }


    public override void OnEnemyProjBodyEntered(Godot.Object body) {
        base.OnEnemyProjBodyEntered(body);
        Player player = (Player)body;
        player.Teleport(levelNode, teleDest);        
    }


}
