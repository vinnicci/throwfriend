using Godot;
using System;

public class SnarkDmgUpgrade : Pickable
{
    public override void OnPickableItemBodyEntered(Godot.Object body) {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.SnarkDmgMult += 1;
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            player.UpdateStatsDisp();
        }
    }



}
