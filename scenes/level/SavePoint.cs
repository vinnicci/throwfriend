using Godot;
using System;


///<summary>Place only on safer/enemy free zones</summary>
public class SavePoint : Area2D
{    
    bool saving = false;
    Player player;

    public Level LevelNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
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
        if(saving == false || player.WeaponNode.CurrentState != Weapon.States.HELD || player.IsDead) {
            return;
        }
        Main mainNode = (Main)GetNode("/root/Main");
        mainNode.SavePlayerData(true);
        player.WarnPlayer("GAME SAVED");
        //turn off process
        SetProcess(false);
    }


}
