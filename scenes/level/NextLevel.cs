using Godot;
using System;

public class NextLevel : Area2D, ILevelObject
{
    Position2D spawnPos;
    String nextLevel;
    Main mainNode;
    bool proceeding = false;
    Player player;

    public String SwitchSignal {get; set;}


    public override void _Ready() {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        spawnPos = (Position2D)GetNode("SpawnPos");
        SwitchSignal = nameof(Switched);
    }


    public Level LevelNode {get; set;}


    void OnNextLevelBodyEntered(Godot.Object body) {
        if(body is Player) {
            EmitSignal(nameof(Switched));
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
            mainNode.GoToLevel(GetRandomLevel(), GetEntrance(), (Player)player, false);
            SetProcess(false);
        }
    }


    String GetRandomLevel() {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)((Resource)mainNode.Saver.Get("world_save_file")).Get("NextLevels");
        if((String)dict[GetPath().ToString()] != "") {
            return (String)dict[GetPath().ToString()];
        }
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Resource)mainNode.Saver.Get("world_save_file")).Get("LevelPool");
        arr.Shuffle();
        String lvl = (String)arr[0];
        arr.RemoveAt(0);
        //current current entrance to next lvl
        LinkToLevel(lvl);
        return lvl;
    }


    public void LinkToLevel(String lvl) {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)((Resource)mainNode.Saver.Get("world_save_file")).Get("NextLevels");
        dict[GetPath().ToString()] = lvl;
    }


    String GetEntrance() {
        String ent = "";
        switch(Name) {
            case "N":
                ent = "S/SpawnPos";
                ShiftCurrentCell(Vector2.Up);
                break;
            case "E":
                ent = "W/SpawnPos";
                ShiftCurrentCell(Vector2.Right);
                break;
            case "W":
                ent = "E/SpawnPos";
                ShiftCurrentCell(Vector2.Left);
                break;
            case "S":
                ent = "N/SpawnPos";
                ShiftCurrentCell(Vector2.Down);
                break;
        }
        return ent;
    }


    void ShiftCurrentCell(Vector2 to) {
        Vector2 currentCell = (Vector2)((Resource)mainNode.Saver.Get("player_save_file")).Get("CurrentCell");
        ((Resource)mainNode.Saver.Get("player_save_file")).Set("CurrentCell", currentCell + to);
    }


    [Signal] public delegate void Switched();


    public void Switch() {
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)((Resource)mainNode.Saver.Get("world_save_file")).Get("NextLevels");
        nextLevel = (String)dict[GetPath().ToString()];
    }


}
