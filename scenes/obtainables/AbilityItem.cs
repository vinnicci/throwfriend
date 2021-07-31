using Godot;
using System;

public class AbilityItem : BaseCollectable
{
    //perma hp + 1
    public override void OnPickableItemBodyEntered(Godot.Object body)
    {
        if(body is Player) {
            animation.Play("pickup");
            Player player = (Player)body;
            player.AvailableUpgrade += 1;
            player.UpdateUpgrade();
        }
    }

    
}
