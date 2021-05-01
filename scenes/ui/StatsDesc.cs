using Godot;
using System;

public class StatsDesc : Control
{
    public AnimationPlayer InGameUIAnim {get; set;}
    public Settings SettingsNode {get; set;}
    public Label DescriptionLabel {get; private set;}

    private Button settingsButton;


    public override void _Ready()
    {
        base._Ready();
        DescriptionLabel = (Label)GetNode("DescPanel/Description");
        settingsButton = (Button)GetNode("DescPanel/SettingsButton");
    }


    private void OnSettingsButtonPressed() {
        if(InGameUIAnim.IsPlaying() == false && SettingsNode.Visible == false) {
            InGameUIAnim.Play("settings_enter");
        }
    }


}
