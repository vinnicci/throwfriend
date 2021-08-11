using Godot;
using System;

public class HealthPickup : Collectable
{
    public override void OnCollectableBodyEntered(Godot.Object body)
    {
        base.OnCollectableBodyEntered(body);
        Player player = (Player)body;
        if(player.Health == player.health) {
            return;
        }
        TriggerAnim.Play("pickup");
        player.Hit(new Vector2(0,0), -1);
        player.UpdateStatsDisp();
    }


}
