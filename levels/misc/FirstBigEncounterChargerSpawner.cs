using Godot;
using System;

class FirstBigEncounterChargerSpawner : BaseChargerSpawner, IQuest
{
    public Main MainNode {get; set;}
    [Export] public String QuestID {get; set;}
    [Export] public String QuestKey {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
    }


    public void CheckQuest() {}


    public override bool Hit(Vector2 knockback, int damage) {
        if(base.Hit(knockback, damage)) {
            if(Health <= 0) {
                UpdateQuest();
            }
            return true;
        }
        return false;
    }


    public void UpdateQuest() {
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(arr.Contains(QuestKey) == false) {
            arr.Add(QuestKey);
            MainNode.Saver.Call("save_world_data");
        }
    }


}
