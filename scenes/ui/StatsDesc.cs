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
    Control helpDisp;
    Label tipLabel;
    Button settingsButton;


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
        helpDisp = (Control)GetNode("StatsPanel/HelpDisp");
        tipLabel = (Label)GetNode("StatsPanel/HelpDisp/Tip");
        currentTip = 5;
        OnNextTipPressed();
    }


    void OnSettingsButtonPressed() {
        if(InGameUIAnim.IsPlaying() == false && SettingsNode.Visible == false) {
            InGameUIAnim.Play("settings_enter");
        }
    }
    

    void OnExitPressed() {
        if(InGameUIAnim.IsPlaying() == false) {
            InGameUIAnim.Play("exit");
        }
    }


    public void UpdateStatsDisp() {
        healthStat.Text = "Current HP: " + PlayerNode.Health;
        maxHealthStat.Text = "Max HP: " + PlayerNode.health;
        speedStat.Text = "Speed: " +
        (PlayerNode.WeaponNode.CurrentState == Weapon.States.HELD ?
        GetDescriptive(PlayerNode.Speed, 0) :
        GetDescriptive(PlayerNode.Speed - Player.EXTRA_SPEED_WITHOUT_WEAPON, 0));
        throwStrengthStat.Text = "Throw Strength: " + GetDescriptive(PlayerNode.ThrowStrength, 1);
        snarkDmgStat.Text = "Snark Damage: " + (PlayerNode.WeaponNode.Damage * PlayerNode.SnarkDmgMult);
    }


    const int BASE_SPEED = 90;
    const int SPEED_INCREMENT = 125;
    const int BASE_THROW = 100;
    const int THROW_INCREMENT = 50;


    String GetDescriptive(int mag, int type) {
        float val = 0;
        if(type == 0) {
            val = (mag - BASE_SPEED)/SPEED_INCREMENT;
        }
        else {
            val = (mag - BASE_THROW)/THROW_INCREMENT;
        }
        if(val <= 0) {
            if(type == 0) return "Slow" + " (" + mag + ")";
            else return "Weak" + " (" + mag + ")";
        }
        else if(val <= 1) {
            return "Moderate" + " (" + mag + ")";
        }
        else {
            if(type == 0) return "Fast" + " (" + mag + ")";
            else return "Strong" + " (" + mag + ")";
        }
    }


    void OnStatsPressed() {
        statsDisp.Visible = true;
        helpDisp.Visible = false;
    }


    void OnHelpPressed() {
        helpDisp.Visible = true;
        statsDisp.Visible = false;
    }


    int currentTip;
    const String path = "StatsPanel/HelpDisp/Tip";
    [Export] Godot.Collections.Array<String> tipsArr;


    void OnNextTipPressed() {
        ((Node2D)GetNode(path + currentTip)).Visible = false;
        currentTip += 1;
        if(currentTip > 5) {
            currentTip = 1;
        }
        tipLabel.Text = tipsArr[currentTip - 1];
        ((Node2D)GetNode(path + currentTip)).Visible = true;
    }


    void OnMainMenuButtonPressed() {
        Main mainNode = (Main)GetNode("/root/Main");
        mainNode.GoToMainMenu();
    }


}
