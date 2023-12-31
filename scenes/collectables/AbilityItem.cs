using Godot;
using System;

public class AbilityItem : Collectable
{
    //perma hp + 1
    public override void OnCollectableBodyEntered(Godot.Object body)
    {
        base.OnCollectableBodyEntered(body);
        TriggerAnim.Play("pickup");
        Player player = (Player)body;
        player.AvailableUpgrade += 1;
        player.UpdateUpgrade();
        player.WarnPlayer("NEW ABILITY AVAILABLE");
        player.PlaySoundEffect("AbilityPickup");
    }


}
