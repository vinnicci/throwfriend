using Godot;
using System;

public class HealthUpgrade : Collectable
{
    public override void OnCollectableBodyEntered(Godot.Object body) {
        base.OnCollectableBodyEntered(body);
        TriggerAnim.Play("pickup");
        Player player = (Player)body;
        player.ChangeEntityBaseStats(player.health + 1, -1);
        player.UpdateStatsDisp();
        player.WarnPlayer("MAX HP INCREASED");
    }


}
