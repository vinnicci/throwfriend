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
            if(IsDead) {
                this.CallDeferred(nameof(SpawnInstance), "ability", 1);
                this.CallDeferred(nameof(SpawnInstance), "dialogue", 1);
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


    public override void SpawnInstance(String packedSceneKey, int count = 1) {
        base.SpawnInstance(packedSceneKey, count);
        if(packedSceneKey == "ability") {
            AbilityItem item = (AbilityItem)spawnScenes[packedSceneKey].Instance();
            item.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        }
        if(packedSceneKey == "dialogue") {
            DialogueTrigger dialogue = (DialogueTrigger)spawnScenes[packedSceneKey].Instance();
            LevelNode.AddChild(dialogue);
            dialogue.GlobalPosition = GlobalPosition;
        }
    }


}
