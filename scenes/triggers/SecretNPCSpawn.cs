using Godot;
using System;

public class SecretNPCSpawn : Position2D
{
    public Main MainNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        CallDeferred(nameof(SpawnNPC));
    }


    public void SpawnNPC() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))["SECRET"];
        if(qArr.Count == 0) {
            SpawnSecretNPCDef("res://scenes/triggers/SecretNPC1.tscn");
        }
        else if(qArr.Count == 3) {
            SpawnSecretNPCDef("res://scenes/triggers/SecretNPC2.tscn");
        }
        else if(qArr.Count == 6) {
            SpawnSecretNPCDef("res://scenes/triggers/SecretNPC3.tscn");
        }
    }


    void SpawnSecretNPCDef(String resName) {
        PackedScene secretNPCPack = (PackedScene)ResourceLoader.Load(resName);
        DialogueTrigger secretNPC = (DialogueTrigger)secretNPCPack.Instance();
        GetParent().AddChild(secretNPC);
        secretNPC.GlobalPosition = GlobalPosition;
    }


}
