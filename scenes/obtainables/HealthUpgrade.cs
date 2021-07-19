using Godot;
using System;

public class HealthUpgrade : BaseObtainable
{
    public override void OnPickableItemBodyEntered(Godot.Object body) {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.ChangeEntityBaseStats(player.health + 1, -1);
            player.UpdateStatsDisp();
        }
    }


}
