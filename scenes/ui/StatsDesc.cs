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
    Label snarkCarrySpeedReduction;
    Control helpDisp;
    Control fasttravelDisp;
    Label tipLabel;
    Button settingsButton;
    Main mainNode;


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
        snarkCarrySpeedReduction = (Label)GetNode("StatsPanel/StatsDisp/SnarkCarrySpeedReduction");
        helpDisp = (Control)GetNode("StatsPanel/HelpDisp");
        tipLabel = (Label)GetNode("StatsPanel/HelpDisp/Tip");
        fasttravelDisp = (Control)GetNode("FastTravelDisp");
        //connect fast travel buttons
        foreach(Button button in fasttravelDisp.GetChildren()) {
            if(button.IsConnected("pressed", this, nameof(OnFastTravePointPressed))) {
                break;
            }
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(button.Name);
            button.Connect("pressed", this, nameof(OnFastTravePointPressed), arr);
        }
        mainNode = (Main)GetNode("/root/Main");
        OnNextTipPressed();
        if(IsInstanceValid(mainNode.WorldSaveFile)) {
            UpdateFastTravelPoints();
        }
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


    void OnMainMenuButtonPressed() {
        mainNode.GoToMainMenu();
    }


    void OnStatsPressed() {
        statsDisp.Visible = true;
        helpDisp.Visible = false;
        fasttravelDisp.Visible = false;
    }


    void OnHelpPressed() {
        helpDisp.Visible = true;
        statsDisp.Visible = false;
        fasttravelDisp.Visible = false;
    }


    void OnFastTravelPressed() {
        if(IsInstanceValid(mainNode.WorldSaveFile) == false) {
            return;
        }
        fasttravelDisp.Visible = true;
        statsDisp.Visible = false;
        helpDisp.Visible = false;
    }


    //stats and description
    public void UpdateStatsDisp() {
        healthStat.Text = "Current HP: " + PlayerNode.Health;
        maxHealthStat.Text = "Max HP: " + PlayerNode.health;
        speedStat.Text = "Speed: " + GetDescriptive(PlayerNode.speed, 0);
        throwStrengthStat.Text = "Throw Strength: " + GetDescriptive(PlayerNode.ThrowStrength, 1);
        snarkDmgStat.Text = "Snark Damage: " + (int)(PlayerNode.SnarkDmg * PlayerNode.SnarkDmgMult);
        snarkCarrySpeedReduction.Text =
        "Snark Carry Speed Reduction: " + (int)(PlayerNode.speed * PlayerNode.SnarkCarrySpeedReduction);
    }


    const int BASE_SPEED = 340;
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


    //help and tips
    int currentTip = 3;
    const String path = "StatsPanel/HelpDisp/Tip";
    [Export] Godot.Collections.Array<String> tipsArr;


    void OnNextTipPressed() {
        ((Node2D)GetNode(path + currentTip)).Visible = false;
        currentTip += 1;
        if(currentTip == 4 && IsInstanceValid(mainNode.WorldSaveFile) &&
        ((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("Quests")).Contains("SECRET")) {
            Godot.Collections.Array qArr =
            (Godot.Collections.Array)((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("Quests"))["SECRET"];
            if(qArr.Count == 7) {
                System.Text.StringBuilder txt = new System.Text.StringBuilder();
                foreach(String s in qArr) {
                    txt.Append(s + " ");
                }
                tipLabel.Text = txt.ToString();
                ((Node2D)GetNode(path + currentTip)).Visible = true;
                return;
            }
            currentTip = 0;
        }
        else if(currentTip > 3) {
            currentTip = 0;
        }
        tipLabel.Text = tipsArr[currentTip];
        ((Node2D)GetNode(path + currentTip)).Visible = true;
    }


    void OnSecretButtonPressed() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("Quests"))["SECRET"];
        if(qArr.Count != 7) {
            return;
        }
        ((AnimationPlayer)GetNode("StatsPanel/HelpDisp/Tip4/Anim")).Play("trigger");
        qArr.Add("NOT SPECIAL");
        mainNode.Saver.Call("save_world_data");
    }


    //fast travel
    void OnFastTravePointPressed(String name) {
        Vector2 cell = (Vector2)GD.Str2Var(name);
        if(cell == (Vector2)mainNode.PlayerSaveFile.Get("CurrentCell")) {
            return;
        }
        else if(PlayerNode.LevelNode.PlayerEngaging.Count != 0) {
            PlayerNode.WarnPlayer("CANNOT PROCEED WHILE ENGAGING ENEMIES");
            return;
        }
        statsDisp.Visible = true;
        fasttravelDisp.Visible = false;
        mainNode.PlayerSaveFile.Set("CurrentCell", cell);
        Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("WorldCells");
        String lvlScn = (String)((Godot.Collections.Dictionary)dict[cell])["scn"];
        mainNode.GoToLevel(lvlScn, "Objects/SavePoint/Pos", PlayerNode, false);
    }


    public void UpdateFastTravelPoints() {
        Godot.Collections.Array arr = (Godot.Collections.Array)mainNode.WorldSaveFile.Get("SavePoints");
        foreach(Vector2 cell in arr) {
            String buttonName = GD.Var2Str(cell);
            ((Button)fasttravelDisp.GetNode(buttonName)).Visible = true;
        }
    }


}
