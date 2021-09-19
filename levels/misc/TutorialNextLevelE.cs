using Godot;
using System;

class TutorialNextLevelE : NextLevel, IQuest
{
    [Export] public string QuestID {get; set;}
    [Export] public String QuestKey {get; set;}
    DialogueTrigger dialogueNode;


    public override void _Ready()
    {
        base._Ready();
        dialogueNode = (DialogueTrigger)GetNode("DialogueTrigger");
    }


    public void CheckQuest() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(qArr.Count == 7) {
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            dialogueNode.QueueFree();
        }
    }


    public void UpdateQuest() {}


}
