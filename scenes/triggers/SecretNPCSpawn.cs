using Godot;
using System;

public class SecretNPCSpawn : Position2D, IQuest
{
    public Main MainNode {get; set;}
    public string QuestID {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        QuestID = "SECRET_ED";
    }


    public void CheckQuest() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(qArr.Count == 0) {
            CallDeferred(nameof(SpawnSecretNPCDef), "res://scenes/triggers/SecretNPC1.tscn", qArr);
        }
        else if(qArr.Count == 1) {

        }
        else if(qArr.Count == 2) {

        }
    }


    void SpawnSecretNPCDef(String resName, Godot.Collections.Array arr) {
        Vector2 currentCell = (Vector2)MainNode.PlayerSaveFile.Get("CurrentCell");
        if(arr.Contains(currentCell)) {
            return;
        }
        PackedScene secretNPCPack = (PackedScene)ResourceLoader.Load(resName);
        SecretNPC secretNPC = (SecretNPC)secretNPCPack.Instance();
        GetParent().AddChild(secretNPC);
        secretNPC.GlobalPosition = GlobalPosition;
        QueueFree();
    }


    public void UpdateQuest() {}


}
