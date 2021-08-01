using Godot;
using System;

public class SnarkDmgUpgrade : Collectable
{
    public override void OnCollectableBodyEntered(Godot.Object body) {
        base.OnCollectableBodyEntered(body);
        anim.Play("pickup");
        Player player = (Player)body;
        player.SnarkDmgMult += 1;
        player.UpdateStatsDisp();
    }


}
