using Godot;
using System;

public class NextLevel : Area2D
{
    bool proceeding = false;
    Player player;

    public Main MainNode {get; set;}
    public Level LevelNode {get; set;}


    public override void _Ready() {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
    }


    void OnNextLevelBodyEntered(Godot.Object body) {
        if(body is Player) {
            player = (Player)body;
            proceeding = true;
            if(LevelNode.PlayerEngaging.Count > 0) {
                player.WarnPlayer("CANNOT PROCEED WHILE ENGAGING ENEMIES");
            }
            else if(player.WeaponNode.CurrentState != Weapon.States.HELD) {
                player.WarnPlayer("YOU MUST CARRY SNARK TO PROCEED");
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
        if(proceeding == false) {
            return;
        }
        if(IsInstanceValid(MainNode.WorldSaveFile) == false) {
            proceeding = false;
            SetProcess(false);
        }
        else if(player.WeaponNode.CurrentState == Weapon.States.HELD && LevelNode.PlayerEngaging.Count == 0) {
            MainNode.GoToLevel(GetLevel(), GetEntrance(), (Player)player, false);
            proceeding = false;
            SetProcess(false);
        }
    }


    String GetLevel() {
        ShiftCurrentCell();
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        Godot.Collections.Dictionary posDict =
        (Godot.Collections.Dictionary)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("WorldCells"))[currentCell];
        return (String)posDict["scn"];
    }


    String GetEntrance() {
        switch(Name) {
            case "N": return "Objects/S/SpawnPos";
            case "E": return "Objects/W/SpawnPos";
            case "W": return "Objects/E/SpawnPos";
            case "S": return "Objects/N/SpawnPos";
        }
        return default;
    }


    void ShiftCurrentCell() {
        Vector2 to = Vector2.Zero;
        switch(Name) {
            case "N": to = Vector2.Up; break;
            case "E": to = Vector2.Right; break;
            case "W": to = Vector2.Left; break;
            case "S": to = Vector2.Down; break;
        }
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        MainNode.PlayerSaveFile.Set("CurrentCell", currentCell + to);
    }


}
