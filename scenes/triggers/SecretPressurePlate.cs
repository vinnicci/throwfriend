using Godot;
using System;

public class SecretPressurePlate : Trigger, IQuest
{
    public Main MainNode {get; set;}
    [Export] public String QuestID {get; set;}
    [Export] public String QuestKey {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
    }


    public void CheckQuest() {
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        String key = currentCell.ToString() + Name;
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)MainNode.LevelSaveFile.Get("Triggers");
        if(dict.Contains(key) && (bool)dict[key] == false) {
            OnSwitchedOn();
        }
    }


    public void UpdateQuest() {
        if(IsInstanceValid(MainNode.WorldSaveFile) == false) {
            return;
        }
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(arr.Contains(QuestKey) == false) {
            arr.Add(QuestKey);
            if(IsInstanceValid(LevelNode.PlayerNode)) {
                LevelNode.PlayerNode.WarnPlayer("WHAT'S THIS BUTTON FOR? (" + arr.Count + "/7)");
            }
            MainNode.Saver.Call("save_world_data");
        }
    }


    public override bool OnSwitchedOn()
    {
        if(base.OnSwitchedOn() == false) {
            return false;
        }
        UpdateQuest();
        return true;
    }


}
