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


    public override void OnEnemyProjBodyEntered(Godot.Object body) {
        base.OnEnemyProjBodyEntered(body);
        Player player = (Player)body;
        if(player.HasNode("ConfuseTimer") == true) {
            confuseTimer = (Timer)player.GetNode("ConfuseTimer");
            confuseTimer.Start();
        }
        else {
            confuseTimer.GetParent().RemoveChild(confuseTimer);
            player.AddChild(confuseTimer);
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(Player.StatusEffect.CONFUSE);
            arr.Add(confuseTimer);
            confuseTimer.Connect("timeout", player, "ClearStatusEffect", arr);
            player.CurrentStatusEffect[(int)Player.StatusEffect.CONFUSE] = true;
            confuseTimer.Start();
        }


    }



}
