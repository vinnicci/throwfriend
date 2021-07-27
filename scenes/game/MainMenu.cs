using Godot;
using System;

public class MainMenu : Control
{
    Main mainNode;
    Control mainUI;
    Control newGameUI;
    Control creditsUI;


    public override void _Ready()
    {
        base._Ready();
        mainNode = (Main)GetNode("/root/Main");
        mainNode.MainMenuNode = this;
        mainNode.GameNode = GetNode("/root/Game");
        mainUI = (Control)GetNode("Menu");
        newGameUI = (Control)GetNode("New");
        creditsUI = (Control)GetNode("Credits");
    }


    async void OnNewPressed() {
        mainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        mainUI.Visible = false;
        newGameUI.Visible = true;
    }


    void OnNewGameYesPressed() {
        newGameUI.Visible = false;
        PackedScene playerPack = (PackedScene)ResourceLoader.Load(Global.PLAYER_SCN);
        mainNode.GoToLevel("TestLevel1", (Player)playerPack.Instance());
    }


    async void OnNewGameNoPressed() {
        mainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        newGameUI.Visible = false;
        mainUI.Visible = true;
    }


    void OnLoadPressed() {
        mainNode.LoadGame();
    }


    async void OnCreditsPressed() {
        mainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        mainUI.Visible = false;
        creditsUI.Visible = true;
    }


    async void OnCreditsBackPressed() {
        mainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        creditsUI.Visible = false;
        mainUI.Visible = true;
    }


    void OnQuitPressed() {
        GetTree().Quit();
    }


}
