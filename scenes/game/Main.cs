using Godot;
using System;

public class Main : Node
{
    public MainMenu MainMenuNode {get; set;}
    public Node GameNode {get; set;}
    public AnimationPlayer FadeAnim {get; set;}

    ColorRect screenColor;
    Level currentLevel;
    

    public override void _Ready()
    {
        base._Ready();
        FadeAnim = (AnimationPlayer)GetNode("CanvasLayer/Anim");
        screenColor = (ColorRect)GetNode("CanvasLayer/ColorRect");
    }


    public void LoadGame() {
        GD.Print("load game!");
    }


    public async void GoToLevel(String newScene, Player player) {
        FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        String spawn = "Start";
        if(IsInstanceValid(currentLevel) == true) {
            Player currPlayer = (Player)currentLevel.GetNodeOrNull("Player");
            if(IsInstanceValid(currPlayer) == true && currPlayer == player) {
                currentLevel.RemoveChild(player);
                spawn = currentLevel.Name + "/SpawnPos";
            }
        }
        CallDeferred(nameof(GoToLevelDef), newScene, player, spawn);
    }


    void GoToLevelDef(String newScene, Player player, String spawn) {
        if(IsInstanceValid(currentLevel) == true) {
            currentLevel.Free();
        }
        PackedScene lvlPack = (PackedScene)ResourceLoader.Load("res://test scenes/" + newScene + ".tscn"); 
        currentLevel = (Level)lvlPack.Instance();
        currentLevel.AddChild(player);
        GameNode.AddChild(currentLevel);
        Position2D spawnPos = (Position2D)currentLevel.GetNode(spawn);
        player.GlobalPosition = spawnPos.GlobalPosition;
    }


    void OnFadeAnimFinished(String animName) {
        if(animName == "fade_in") {
            FadeAnim.Play("fade_out");
        }
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        QueueFree();
    }


}
