using Godot;
using System;

public class MainMenu : Control
{
    public Main MainNode {get; set;}
    Control mainUI;
    Control newGameUI;
    Control creditsUI;


    public override void _Ready()
    {
        base._Ready();
        mainUI = (Control)GetNode("Main");
        newGameUI = (Control)GetNode("New");
        creditsUI = (Control)GetNode("Credits");
    }


    async void OnNewPressed() {
        MainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        mainUI.Visible = false;
        newGameUI.Visible = true;
    }


    void OnNewGameYesPressed() {
        MainNode.NewGame();
    }


    async void OnNewGameNoPressed() {
        MainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        newGameUI.Visible = false;
        mainUI.Visible = true;
    }


    void OnLoadPressed() {
        MainNode.LoadGame();
    }


    async void OnCreditsPressed() {
        MainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        mainUI.Visible = false;
        creditsUI.Visible = true;
    }


    async void OnCreditsBackPressed() {
        MainNode.FadeAnim.Play("fade_in");
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        creditsUI.Visible = false;
        mainUI.Visible = true;
    }


    void OnQuitPressed() {
        GetTree().Quit();
    }


}
