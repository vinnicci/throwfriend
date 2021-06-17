using Godot;
using System;

public class AbilityItem : Pickable
{
    //perma hp + 1
    public override void OnPickableItemBodyEntered(Godot.Object body)
    {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.AvailableUpgrade += 1;
            player.UpdateUpgrade();
            player.ChangeEntityBaseStats(player.health + 1, -1);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            player.UpdateStatsDisp();
        }
    }

    
}
