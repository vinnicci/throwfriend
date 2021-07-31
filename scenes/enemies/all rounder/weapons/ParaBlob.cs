using Godot;
using System;

public class ParaBlob : EnemyProj
{
    Timer confuseTimer;


    public override void _Ready()
    {
        base._Ready();
        confuseTimer = (Timer)GetNode("ConfuseTimer");
    }


    public override bool OnEnemyProjBodyEntered(Godot.Object body) {
        if(base.OnEnemyProjBodyEntered(body) == false) {
            return false;
        }
        Player player = (Player)body;
        if(player.HasNode("ConfuseTimer")) {
            confuseTimer = (Timer)player.GetNode("ConfuseTimer");
            confuseTimer.Start();
        }
        else {
            confuseTimer.GetParent().RemoveChild(confuseTimer);
            player.AddChild(confuseTimer);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(Player.StatusEffect.CONFUSE);
            arr.Add(confuseTimer);
            if(confuseTimer.IsConnected("timeout", player, nameof(Player.ClearStatusEffect)) == false) {
                confuseTimer.Connect("timeout", player, nameof(Player.ClearStatusEffect), arr);
            }
            player.CurrentStatusEffect[(int)Player.StatusEffect.CONFUSE] = true;
            confuseTimer.Start();
        }
        return true;
    }


}
