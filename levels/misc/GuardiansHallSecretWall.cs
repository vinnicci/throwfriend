using Godot;
using System;

public class GuardiansHallSecretWall : LevelTiles, IQuest
{
    [Export] public string QuestID {get; set;}
    [Export] public String QuestKey {get; set;}
    public Main MainNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
    }


    public void CheckQuest() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(qArr.Count == 8) {
            QueueFree();
        }
    }


    public void UpdateQuest() {}


}
