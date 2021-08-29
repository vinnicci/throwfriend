using Godot;
using System;

public class SecretNPC : Trigger, IQuest
{
    public Main MainNode {get; set;}
    [Export] public String QuestID {get; set;}
    [Export] public String QuestKey {get; set;}
    


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        CheckQuest();
        if(IsQueuedForDeletion()) {
            return;
        }
        MainNode.InitLevelObject(this, "Triggers");
        
    }


    public void CheckQuest() {
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        String key = currentCell.ToString() + Name; //GetPath().ToString();
        Godot.Collections.Dictionary dict =
        (Godot.Collections.Dictionary)MainNode.LevelSaveFile.Get("Triggers");
        if(dict.Contains(key) && (bool)dict[key] == false) {
            QueueFree();
        }
    }


    public void UpdateQuest() {
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(arr.Contains(QuestKey) == false) {
            arr.Add(QuestKey);
            MainNode.Saver.Call("save_world_data");
        }
        //play idle after trigger anim is done playing
        TriggerAnim.Play("idle");
    }


    public override void OnSwitchedOn()
    {
        base.OnSwitchedOn();
        UpdateQuest();
    }


}
