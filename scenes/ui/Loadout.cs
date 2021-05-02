using Godot;
using System;

public class Loadout : Control
{
    public Player PlayerNode {get; set;}
    public Weapon WeaponNode {get; set;}
    public ItemSlot Slot1 {get; private set;}
    public ItemSlot Slot2 {get; private set;}
    public ItemSlot Slot3 {get; private set;}
    public ItemSlot Slot4 {get; private set;}
    public ConfigFile ConfigFile {get; private set;}

    private Button slot1KeyBind;
    private Button slot2KeyBind;
    private Button slot3KeyBind;
    private Button slot4KeyBind;
    const String CONFIG = "user://config/config.ini";


    public override void _Ready()
    {
        base._Ready();
        ConfigFile = new ConfigFile();
        Slot1 = (ItemSlot)GetNode("PlayerPanel/ItemSlot");
        Slot2 = (ItemSlot)GetNode("PlayerPanel/ItemSlot2");
        Slot3 = (ItemSlot)GetNode("WeaponPanel/ItemSlot");
        Slot4 = (ItemSlot)GetNode("WeaponPanel/ItemSlot2");
        ConnectButtons();
        InitKeybinds();
        LoadConfig();
    }


    private void ConnectButtons() {
        for(int i = 1; i <= 4; i++) {
            Godot.Collections.Array arr = new Godot.Collections.Array();
            String currentKeyBind = "slot" + i + "KeyBind";
            ItemSlot slot = (ItemSlot)Get("Slot" + i);
            Set(currentKeyBind, slot.GetNode("Button"));
            Button button = (Button)Get(currentKeyBind);
            arr.Add(button);
            button.Connect("pressed", this, "OnKeyBindPressed", arr);
            button.FocusMode = FocusModeEnum.None;
        }
    }


    Godot.Collections.Dictionary keybinds = new Godot.Collections.Dictionary();


    private void InitKeybinds() {
        String[] controls = {"hotkey_1", "hotkey_2", "hotkey_3", "hotkey_4"};
        for(int i = 0; i <= controls.Length-1; i++) {
            Godot.Collections.Dictionary dict = new Godot.Collections.Dictionary();
            switch(controls[i]) {
                case "hotkey_1": 
                    InitKeybind(controls[i], (int)KeyList.Key1, (KeyRebindButton)slot1KeyBind, dict); break;
                case "hotkey_2":
                    InitKeybind(controls[i], (int)KeyList.Key2, (KeyRebindButton)slot2KeyBind, dict); break;
                case "hotkey_3":
                    InitKeybind(controls[i], (int)KeyList.Key3, (KeyRebindButton)slot3KeyBind, dict); break;
                case "hotkey_4":
                    InitKeybind(controls[i], (int)KeyList.Key4, (KeyRebindButton)slot4KeyBind, dict); break;
                default:
                    GD.PrintErr("Error found in SettingsNode."); break;
            }
            keybinds.Add(controls[i], dict);
        }
    }


    private void InitKeybind(String key, int keyPress, KeyRebindButton button, Godot.Collections.Dictionary dict) {
        dict.Add("default", keyPress);
        dict.Add("button", button);
        button.MainNode = this;
        button.Key = key;
    }


    private void LoadConfig() {
        if(ConfigFile.Load(CONFIG) != Error.Ok) {
            GD.PrintErr("Loadout Node can't load the config file.");
            return;
        }
        foreach(String key in keybinds.Keys) {
            if(ConfigFile.HasSectionKey("keybinds", key) == false) {
                Godot.Collections.Dictionary val = (Godot.Collections.Dictionary)keybinds[key];
                ConfigFile.SetValue("keybinds", key, val["default"]);
            }
            ReplaceAction(key, (int)ConfigFile.GetValue("keybinds", key));
            UpdateUIKey(key);
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


    public void SaveConfig() {
        ConfigFile.Save(CONFIG);
    }

    
    public void UpdateSlotIcon(int slotNum) {
        switch(slotNum) {
            case 1: Slot1.UpdateIcon(PlayerNode.Item1); break;
            case 2: Slot2.UpdateIcon(PlayerNode.Item2); break;
            case 3: Slot3.UpdateIcon(WeaponNode.Item1); break;
            case 4: Slot4.UpdateIcon(WeaponNode.Item2); break;
            default: GD.PrintErr("Slot doesn't exist."); break;
        }
    }


    private void OnKeyBindPressed(KeyRebindButton button) {
        button.Text = "PRESS A KEY...";
        button.Pressed = true;
    }


}
