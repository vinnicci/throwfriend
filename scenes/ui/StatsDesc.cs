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
        speedStat.Text = "Speed: " + GetDescriptive(PlayerNode.Speed, 0);
        throwStrengthStat.Text = "Throw Strength: " + GetDescriptive(PlayerNode.ThrowStrength, 1);
        snarkDmgStat.Text = "Snark Damage: " + (PlayerNode.WeaponNode.Damage * PlayerNode.SnarkDmgMult);
    }


    const int SPEED_INCREMENT = 125;
    const int THROW_INCREMENT = 75;


    String GetDescriptive(int mag, int type) {
        float val = 0;
        if(type == 0) {
            val = mag/SPEED_INCREMENT;
        }
        else {
            val = mag/THROW_INCREMENT;
        }
        if(val <= 1) {
            if(type == 0) return "Slow";
            else return "Weak";
        }
        else if(val <= 2) {
            return "Moderate";
        }
        else {
            if(type == 0) return "Fast";
            else return "Strong";
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


    void OnNextTipPressed() {
        ((Node2D)GetNode(path + currentTip)).Visible = false;
        switch(currentTip) {
            case 1: {
                currentTip = 2;
                tipLabel.Text = "Snark can only inflict damage when he's active. You will know he's active when his mouth is open.";
            }
            break;
            case 2: {
                currentTip = 3;
                tipLabel.Text = "You can bind active abilities with the same key, allowing you to perform some interesting moves.";
            }
            break;
            case 3: {
                currentTip = 4;
                tipLabel.Text = "Selecting an ability is permanent and irreversible. Choose wisely.";
            }
            break;
            case 4: {
                currentTip = 5;
                tipLabel.Text = "Look out for any secret paths. These will lead you to areas with rewards at the end.";
            }
            break;
            case 5: {
                currentTip = 1;
                tipLabel.Text = "Releasing Snark increases mobility.";
            }
            break;
        }
        ((Node2D)GetNode(path + currentTip)).Visible = true;
    }


}
