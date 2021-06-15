using Godot;
using System;

public class HealthUpgrade : Pickable
{
    public override void OnPickableItemBodyEntered(Godot.Object body) {
        animation.Play("pickup");
        Player player = (Player)body;
        player.ChangeEntityBaseStats(player.health + 1, -1);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
    }


}
