using Godot;
using System;

public class SecretNPC : Trigger, IQuest
{
    public Main MainNode {get; set;}
    public string QuestID {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        QuestID = "SECRET_ED";
        MainNode.InitLevelObject(this, "Triggers");
    }


    public void CheckQuest() {}


    public void UpdateQuest() {
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        if(arr.Contains(currentCell) == false) {
            arr.Add(currentCell);
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
