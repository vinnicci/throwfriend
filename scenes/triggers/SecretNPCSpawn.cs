using Godot;
using System;

public class SecretNPCSpawn : Position2D
{
    public Main MainNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        MainNode = (Main)GetNode("/root/Main");
        CallDeferred(nameof(SpawnNPCDef));
    }


    public void SpawnNPCDef() {
        if(IsInstanceValid(MainNode.WorldSaveFile) == false) {
            return;
        }
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))["SECRET"];
        if(qArr.Count == 0) {
            SpawnSecretNPC("res://scenes/triggers/SecretNPC1.tscn");
        }
        else if(qArr.Count == 2) {
            SpawnSecretNPC("res://scenes/triggers/SecretNPC2.tscn");
        }
        else if(qArr.Count == 4) {
            SpawnSecretNPC("res://scenes/triggers/SecretNPC3.tscn");
        }
        else if(qArr.Count == 6) {
            SpawnSecretNPC("res://scenes/triggers/SecretNPC4.tscn");
        }
    }


    void SpawnSecretNPC(String resName) {
        PackedScene secretNPCPack = (PackedScene)ResourceLoader.Load(resName);
        DialogueTrigger secretNPC = (DialogueTrigger)secretNPCPack.Instance();
        GetParent().AddChild(secretNPC);
        secretNPC.GlobalPosition = GlobalPosition;
    }


}
