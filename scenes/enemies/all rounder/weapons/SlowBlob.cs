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


    public override void OnEnemyProjBodyEntered(Godot.Object body) {
        base.OnEnemyProjBodyEntered(body);
        Player player = (Player)body;
        if(player.HasNode("SlowTimer") == true) {
            slowTimer = (Timer)player.GetNode("SlowTimer");
            slowTimer.Start();
        }
        else {
            slowTimer.GetParent().RemoveChild(slowTimer);
            player.AddChild(slowTimer);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(Player.StatusEffect.SLOW);
            arr.Add(slowTimer);
            slowTimer.Connect("timeout", player, "ClearStatusEffect", arr);
            player.CurrentStatusEffect[(int)Player.StatusEffect.SLOW] = true;
            slowTimer.Start();
        }


    }


}
