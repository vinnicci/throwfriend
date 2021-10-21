using Godot;
using System;


///<summary>Place only on safer/enemy free zones</summary>
public class SavePoint : Area2D
{    
    bool saving = false;
    Player player;
    Main mainNode;
    Particles2D particles;
    AudioStreamPlayer2D sound;

    public Level LevelNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        particles = (Particles2D)GetNode("Particles2D");
        sound = (AudioStreamPlayer2D)GetNode("Sound");
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
        particles.GlobalPosition = player.GlobalPosition;
        particles.Amount = 100;
        particles.Emitting = true;
        Vector2 currentCell = (Vector2)mainNode.PlayerSaveFile.Get("CurrentCell");
        Godot.Collections.Array arr = (Godot.Collections.Array)mainNode.WorldSaveFile.Get("SavePoints");
        if(arr.Contains(currentCell) == false) {
            arr.Add(currentCell);
            player.UpdateFastTravelPoints();
        }
        sound.Play();
        mainNode.SavePlayerData(true);
        saving = false;
        SetProcess(false);
    }


}
