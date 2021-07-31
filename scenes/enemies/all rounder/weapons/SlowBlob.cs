using Godot;
using System;

public class SlowBlob : EnemyProj
{
    Timer slowTimer;


    public override void _Ready()
    {
        base._Ready();
        slowTimer = (Timer)GetNode("SlowTimer");
    }


    public override bool OnEnemyProjBodyEntered(Godot.Object body) {
        if(base.OnEnemyProjBodyEntered(body) == false) {
            return false;
        }
        Player player = (Player)body;
        if(player.HasNode("SlowTimer")) {
            slowTimer = (Timer)player.GetNode("SlowTimer");
            slowTimer.Start();
        }
        else {
            slowTimer.GetParent().RemoveChild(slowTimer);
            player.AddChild(slowTimer);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(Player.StatusEffect.SLOW);
            arr.Add(slowTimer);
            if(slowTimer.IsConnected("timeout", player, nameof(Player.ClearStatusEffect)) == false) {
                slowTimer.Connect("timeout", player, nameof(Player.ClearStatusEffect), arr);
            }
            player.CurrentStatusEffect[(int)Player.StatusEffect.SLOW] = true;
            slowTimer.Start();
        }
        return true;
    }


}
