using Godot;
using System;

public class Main : Node2D
{
    MainMenu mainMenu;
    [Export] PackedScene level;
    public AnimationPlayer FadeAnim {get; set;}
    ColorRect screenColor;
    Timer resumeTimer;


    public override void _Ready()
    {
        base._Ready();
        mainMenu = (MainMenu)GetNode("MainMenu");
        mainMenu.MainNode = this;
        FadeAnim = (AnimationPlayer)GetNode("CanvasLayer/Anim");
        screenColor = (ColorRect)GetNode("CanvasLayer/ColorRect");
        resumeTimer = (Timer)GetNode("Resume");
    }


    public async void NewGame() {
        FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        mainMenu.Visible = false;
        Level lvl = (Level)level.Instance();
        AddChild(lvl);
        
    }


    public void LoadGame() {
        GD.Print("load game!");
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
