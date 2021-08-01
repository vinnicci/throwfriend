using Godot;
using System;

public class HealthUpgrade : Collectable
{
    public override void OnCollectableBodyEntered(Godot.Object body) {
        base.OnCollectableBodyEntered(body);
        anim.Play("pickup");
        Player player = (Player)body;
        player.ChangeEntityBaseStats(player.health + 1, -1);
        player.UpdateStatsDisp();
    }


}