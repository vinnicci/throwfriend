using Godot;
using System;

public class SnarkDmgUpgrade : BaseObtainable
{
    public override void OnPickableItemBodyEntered(Godot.Object body) {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.SnarkDmgMult += 1;
            player.UpdateStatsDisp();
        }
    }


}
