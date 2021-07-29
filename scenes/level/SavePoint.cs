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
        if(saving == false || player.WeaponNode.CurrentState != Weapon.States.HELD || player.IsDead == true) {
            return;
        }
        Main mainNode = (Main)GetNode("/root/Main");
        Godot.Collections.Dictionary dict = new Godot.Collections.Dictionary();
        //add relevant data to save in the dictionary
        //current level
        dict.Add("Level", LevelNode.Filename);
        //hp
        player.ChangeEntityBaseStats(player.health, -1);
        dict.Add("MaxHP", player.Health);
        //snark dmg mult
        dict.Add("SnarkDmgMult", player.SnarkDmgMult);
        //available upgrades
        dict.Add("AvailableUpgrades", player.AvailableUpgrade);
        //player item 1
        Item item = player.Item1;
        if(IsInstanceValid(item) == true) {
            dict.Add("PlayerItem1", item.Filename);
        }
        else {
            dict.Add("PlayerItem1", "");
        }
        //player item 2
        item = player.Item2;
        if(IsInstanceValid(item) == true) {
            dict.Add("PlayerItem2", item.Filename);
        }
        else {
            dict.Add("PlayerItem2", "");
        }
        //weap item 1
        item = player.WeaponNode.Item1;
        if(IsInstanceValid(item) == true) {
            dict.Add("WeapItem1", item.Filename);
        }
        else {
            dict.Add("WeapItem1", "");
        }
        //weap item 2
        item = player.WeaponNode.Item2;
        if(IsInstanceValid(item) == true) {
            dict.Add("WeapItem2", item.Filename);
        }
        else {
            dict.Add("WeapItem2", "");
        }
        player.UpdateStatsDisp();
        //call save function in Main Singleton
        mainNode.SaveData(dict);
        player.WarnPlayer("GAME SAVED");
        //turn off process
        SetProcess(false);
    }


}
