using Godot;
using System;

public class KeyRebindButton : Button
{
    public String Key {get; set;}
    private Control mainNode;
    public Control MainNode {
        get {
            return mainNode;
        }
        set {
            mainNode = value;
            if(MainNode is Settings) {
                Settings settings = (Settings)MainNode;
                configFile = settings.ConfigFile;
            }
            else if(MainNode is Loadout) {
                Loadout loadout = (Loadout)MainNode;
                configFile = loadout.ConfigFile;
            }
        }
    }
    
    private ConfigFile configFile;


    public override void _Ready()
    {
        base._Ready();
        FocusMode = FocusModeEnum.None;
    }


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(Text != "PRESS A KEY...") {
            return;
        }
        if(@event is InputEventKey) {
            InputEventKey eventKey = (InputEventKey)@event;
            if((uint)eventKey.Scancode == (uint)KeyList.Escape) {
                Rebind((int)configFile.GetValue("keybinds", Key));
            }
            else {
                Rebind((int)eventKey.Scancode);
            }
        }
        else if(@event is InputEventMouseButton) {
            InputEventMouseButton eventKey = (InputEventMouseButton)@event;
            Rebind(eventKey.ButtonIndex);
        }
    }


    private void Rebind(int buttonCode) {
        configFile.SetValue("keybinds", Key, buttonCode);
        if(MainNode is Settings) {
            Settings settings = (Settings)MainNode;
            settings.ReplaceAction(Key, (int)configFile.GetValue("keybinds", Key));
            settings.UpdateUIKey(Key);
            settings.SaveConfig();
        }
        else if(MainNode is Loadout) {
            Loadout loadout = (Loadout)MainNode;
            loadout.ReplaceAction(Key, (int)configFile.GetValue("keybinds", Key));
            loadout.UpdateUIKey(Key);
            loadout.SaveConfig();
        }
        Pressed = false;
    }


}
