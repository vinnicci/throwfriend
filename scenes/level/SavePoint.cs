using Godot;
using System;


///<summary>Place only on safer/enemy free zones</summary>
public class SavePoint : Area2D
{    
    bool saving = false;
    Player player;
    Main mainNode;

    public Level LevelNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        SetProcess(false);
    }


    void OnSavePointBodyEntered(Godot.Object body) {
        if(body is Player == false) {
            return;
        }
        player = (Player)body;
        saving = true;
        SetProcess(true);
    }


    void OnSavePointBodyExited(Godot.Object body) {
        if(body is Player == false) {
            return;
        }
        saving = false;
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(saving == false || player.WeaponNode.CurrentState != Weapon.States.HELD || player.Health <= 0) {
            return;
        }
        if(IsInstanceValid(mainNode.PlayerSaveFile) == false) {
            saving = false;
            SetProcess(false);
            return;
        }
        player.WarnPlayer("GAME SAVED");
        Vector2 currentCell = (Vector2)mainNode.PlayerSaveFile.Get("CurrentCell");
        Godot.Collections.Array arr = (Godot.Collections.Array)mainNode.WorldSaveFile.Get("SavePoints");
        if(arr.Contains(currentCell) == false) {
            arr.Add(currentCell);
            player.UpdateFastTravelPoints();
        }
        mainNode.SavePlayerData(true);
        SetProcess(false);
    }


}
