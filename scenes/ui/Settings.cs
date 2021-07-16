using Godot;
using System;

public class Settings : Control
{
    public AnimationPlayer InGameUIAnim {get; set;}
    public ConfigFile ConfigFile {get; private set;}

    const String CONFIG = "user://config/config.ini";


    public override void _Ready()
    {
        base._Ready();
        ConfigFile = new ConfigFile();
        ConnectButtons();
        InitKeybinds();
        LoadConfig();
    }


    void ConnectButtons() {
        Panel panel = (Panel)GetNode("Panel");
        for(int i = 0; i <= panel.GetChildren().Count-1; i++) {
            Node obj = panel.GetChild(i);
            Button button;
            Godot.Collections.Array arr;
            if(obj is Label && obj.IsInGroup("Key") == true) {
                button = (Button)obj.GetChild(0);
            }
            else if(obj is Button) {
                button = (Button)obj;
            }
            else {
                continue;
            }
            button.FocusMode = FocusModeEnum.None;
            arr = new Godot.Collections.Array();
            arr.Add(obj.Name);
            arr.Add(button);
            button.Connect("pressed", this, nameof(OnButtonPressed), arr);
        }
    }


    Godot.Collections.Dictionary keybinds = new Godot.Collections.Dictionary();


    void InitKeybinds() {
        String[] controls = {"up", "down", "left", "right", "throw_weap", "in_game_ui"};
        for(int i = 0; i <= controls.Length-1; i++) {
            Godot.Collections.Dictionary dict = new Godot.Collections.Dictionary();
            switch(controls[i]) {
                case "up":
                    InitKeybind(controls[i], (int)KeyList.W, (KeyRebindButton)GetNode("Panel/Up/Button"), dict); break;
                case "down":
                    InitKeybind(controls[i], (int)KeyList.S , (KeyRebindButton)GetNode("Panel/Down/Button"), dict); break;
                case "left":
                    InitKeybind(controls[i], (int)KeyList.A, (KeyRebindButton)GetNode("Panel/Left/Button"), dict); break;
                case "right":
                    InitKeybind(controls[i], (int)KeyList.D, (KeyRebindButton)GetNode("Panel/Right/Button"), dict); break;
                case "throw_weap":
                    InitKeybind(controls[i], (int)ButtonList.Left, (KeyRebindButton)GetNode("Panel/Throw/Button"), dict); break;
                case "in_game_ui":
                    InitKeybind(controls[i], (int)KeyList.Tab, (KeyRebindButton)GetNode("Panel/Menu/Button"), dict); break;
                default:
                    GD.PrintErr("Error found in SettingsNode."); break;
            }
            keybinds.Add(controls[i], dict);
        }
    }


    void InitKeybind(String key, int keyPress, KeyRebindButton button, Godot.Collections.Dictionary dict) {
        dict.Add("default", keyPress);
        dict.Add("button", button);
        button.MainNode = this;
        button.Key = key;
    }


    void LoadConfig() {
        if(ConfigFile.Load(CONFIG) != Error.Ok) {
            InitIniFile();
        }
        foreach(String key in keybinds.Keys) {
            ReplaceAction(key, (int)ConfigFile.GetValue("keybinds", key));
            UpdateUIKey(key);
        }
    }


    void InitIniFile() {
        Directory directory = new Directory();
        File iniFile = new File();
        if(directory.DirExists("user://config/") == false) {
            directory.MakeDirRecursive("user://config/");
        }
        iniFile.Open(CONFIG, File.ModeFlags.Write);
        iniFile.Close();
        if(ConfigFile.Load(CONFIG) == Error.Ok) {
            foreach(String key in keybinds.Keys) {
                Godot.Collections.Dictionary val = (Godot.Collections.Dictionary)keybinds[key];
                ConfigFile.SetValue("keybinds", key, val["default"]);
            }
        }
	    SaveConfig();
    }


    public void SaveConfig() {
        ConfigFile.Save(CONFIG);
    }


    void ResetToDefault() {
        foreach(String key in keybinds.Keys) {
            Godot.Collections.Dictionary val = (Godot.Collections.Dictionary)keybinds[key];
            ConfigFile.SetValue("keybinds", key, val["default"]);
            ReplaceAction(key, (int)ConfigFile.GetValue("keybinds", key));
            UpdateUIKey(key);
        }
    }


    public void ReplaceAction(String key, int buttonCode) {
        Godot.Collections.Array actionList = InputMap.GetActionList(key);
        if(actionList.Count != 0) {
            InputMap.ActionEraseEvent(key, (InputEvent)actionList[0]);
        }
        if(buttonCode == -1) {
            return;
        }
        else if(buttonCode <= 7) {
            InputEventMouseButton newKey = new InputEventMouseButton();
            newKey.ButtonIndex = buttonCode;
            InputMap.ActionAddEvent(key, newKey);
        }
        else {
            InputEventKey newKey = new InputEventKey();
            newKey.Scancode = (uint)buttonCode;
            InputMap.ActionAddEvent(key, newKey);
        }
    }


    public void UpdateUIKey(String key) {
        Godot.Collections.Dictionary keyDict = (Godot.Collections.Dictionary)keybinds[key];
        Button button = (Button)keyDict["button"];
        int val = (int)ConfigFile.GetValue("keybinds", key);
        if (val == -1) {
            button.Text = "UNASSIGNED";
        }
        else if(val <= 7) {
            button.Text = "Mouse" + val.ToString();
        }
        else {
            button.Text = OS.GetScancodeString((uint)val);
        }
    }



    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Input.IsActionJustPressed("in_game_ui") && Visible == true && IsInstanceValid(InGameUIAnim) == true &&
        InGameUIAnim.IsPlaying() == false) {
            InGameUIAnim.Play("settings_exit");
        }
    }


    void OnButtonPressed(String slotName, Button button) {
        switch(slotName) {
            case "Up":
            case "Down":
            case "Left":
            case "Right":
            case "Throw":
            case "Menu": {
                button.Text = "PRESS A KEY...";
                button.Pressed = true;
            }
            break;
            case "Back": {
                if(InGameUIAnim.IsPlaying() == false) {
                    InGameUIAnim.Play("settings_exit");
                }
            }
            break;
            case "Default": {
                ResetToDefault();
            }
            break;
            default: {
                GD.PrintErr("Wrong object reference in SettingsNode.");
            }
            break;
        }
    }


}
