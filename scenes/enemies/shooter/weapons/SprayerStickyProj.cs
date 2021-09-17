using Godot;
using System;

public class SprayerStickyProj : SprayerProj
{
    Player player;


    public override bool OnEnemyProjBodyEntered(Godot.Object body) {
        player = (Player)body;
        if(IsInstanceValid(player) == false) {
            return false;
        }
        player.CurrentStatusEffect[(int)Player.StatusEffect.SLOW] = true;
        return true;
    }


    public void OnSprayerAcidProjBodyExited(Godot.Object body) {
        if(IsInstanceValid(player) == false) {
            return;
        }
        player.ClearStatusEffect(Player.StatusEffect.SLOW, default);
        player = default;
    }


    public override void ReturnToPool() {
        base.ReturnToPool();
    }


}
