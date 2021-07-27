using Godot;
using System;

public class NextLevel : Area2D
{
    Position2D spawnPos;
    Main mainNode;
    bool proceeding = false;
    Player player;


    public override void _Ready() {
        base._Ready();
        spawnPos = (Position2D)GetNode("SpawnPos");
    }


    void OnNextLevelBodyEntered(Godot.Object body) {
        if(body is Player) {
            player = (Player)body;
            proceeding = true;
            if(player.WeaponNode.CurrentState != Weapon.States.HELD) {
                player.WarnPlayer("YOU MUST CARRY SNARK TO PROCEED");
                return;
            }
        }
    }


    void OnNextLevelBodyExited(Godot.Object body) {
        if(body is Player) {
            proceeding = false;
        }
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(proceeding == true && player.WeaponNode.CurrentState == Weapon.States.HELD) {
            mainNode = (Main)GetNode("/root/Main");
            mainNode.GoToLevel(Name, (Player)player);
            SetProcess(false);
        }
    }


}
