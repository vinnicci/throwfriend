using Godot;
using System;

public class NextLevel : Area2D
{
    Position2D spawnPos;
    [Export] String nextLevel;
    Main mainNode;
    bool proceeding = false;
    Player player;


    public override void _Ready() {
        base._Ready();
        spawnPos = (Position2D)GetNode("SpawnPos");
    }


    public Level LevelNode {get; set;}


    void OnNextLevelBodyEntered(Godot.Object body) {
        if(body is Player) {
            player = (Player)body;
            proceeding = true;
            if(player.WeaponNode.CurrentState != Weapon.States.HELD) {
                player.WarnPlayer("YOU MUST CARRY SNARK TO PROCEED");
            }
            else if(LevelNode.PlayerEngaging > 0) {
                player.WarnPlayer("CAN'T PROCEED WHILE ENGAGING WITH ENEMIES");
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
        if(proceeding && player.WeaponNode.CurrentState == Weapon.States.HELD && LevelNode.PlayerEngaging == 0) {
            mainNode = (Main)GetNode("/root/Main");
            mainNode.GoToLevel(nextLevel, LevelNode.Name+"/SpawnPos", (Player)player, false);
            SetProcess(false);
        }
    }


}
