using Godot;
using System;

public class TeleBlob : EnemyProj
{
    Vector2 teleDest;


    public override void Spawn(Level lvl, Vector2 globalPos, Vector2 destination,
    float globalRotDeg = 0, bool homeToPlayer = false) {
        base.Spawn(lvl, globalPos, destination, globalRotDeg, homeToPlayer);
        teleDest = GlobalPosition;
    }


    public override void OnEnemyProjBodyEntered(Godot.Object body) {
        base.OnEnemyProjBodyEntered(body);
        Player player = (Player)body;
        player.Teleport(levelNode, teleDest);        
    }


}
