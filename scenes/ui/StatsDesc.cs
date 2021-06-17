using Godot;
using System;

public class StatsDesc : Control
{
    public AnimationPlayer InGameUIAnim {get; set;}
    public Settings SettingsNode {get; set;}
    public Label DescriptionLabel {get; private set;}
    public Player PlayerNode {get; set;}
    VBoxContainer statsDisp;
    Label healthStat;
    Label maxHealthStat;
    Label speedStat;
    Label throwStrengthStat;
    Label snarkDmgStat;

    private Button settingsButton;


    public override void _Ready()
    {
        base._Ready();
        DescriptionLabel = (Label)GetNode("DescPanel/Description");
        settingsButton = (Button)GetNode("DescPanel/SettingsButton");
        statsDisp = (VBoxContainer)GetNode("StatsPanel/StatsDisp");
        healthStat = (Label)GetNode("StatsPanel/StatsDisp/Health");
        maxHealthStat = (Label)GetNode("StatsPanel/StatsDisp/MaxHealth");
        speedStat = (Label)GetNode("StatsPanel/StatsDisp/Speed");
        throwStrengthStat = (Label)GetNode("StatsPanel/StatsDisp/ThrowStrength");
        snarkDmgStat = (Label)GetNode("StatsPanel/StatsDisp/SnarkDmg");
    }


    private void OnSettingsButtonPressed() {
        if(InGameUIAnim.IsPlaying() == false && SettingsNode.Visible == false) {
            InGameUIAnim.Play("settings_enter");
        }
    }
    

    private void OnExitPressed() {
        if(InGameUIAnim.IsPlaying() == false) {
            InGameUIAnim.Play("exit");
        }
    }


    public void UpdateStatsDisp() {
        healthStat.Text = "Current HP: " + PlayerNode.Health;
        maxHealthStat.Text = "Max HP: " + PlayerNode.health;
        speedStat.Text = "Speed: " + GetDescriptive(PlayerNode.Speed, 0);
        throwStrengthStat.Text = "Throw Strength: " + GetDescriptive(PlayerNode.ThrowStrength, 1);
        snarkDmgStat.Text = "Snark Damage: " + (PlayerNode.WeaponNode.Damage * PlayerNode.SnarkDmgMult);
    }


    const int INCREMENT = 125;


    private String GetDescriptive(int mag, int type) {
        float val = mag/INCREMENT;
        if(val <= 1) {
            if(type == 0) return "Slow";
            else return "Weak";
        }
        else if(val <= 2) {
            return "Moderate";
        }
        else {
            if(type == 0) return "Strong";
            else return "Fast";
        }
    }


}
