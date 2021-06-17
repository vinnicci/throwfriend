using Godot;
using System;

public class HealthPickup : Pickable
{
    public override void OnPickableItemBodyEntered(Godot.Object body)
    {
        if(body is Player) {
            Player player = (Player)body;
            if(player.Health == player.health) {
                return;
            }
            animation.Play("pickup");
            player.Hit(new Vector2(0,0), -1);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            player.UpdateStatsDisp();
        }
    }


}
