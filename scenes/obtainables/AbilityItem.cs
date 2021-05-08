using Godot;
using System;

public class AbilityItem : Pickable
{
    public override void OnPickableItemBodyEntered(Godot.Object body)
    {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.AvailableUpgrade += 1;
            player.UpdateUpgrade();
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        }
    }
}
