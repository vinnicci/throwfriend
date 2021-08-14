using Godot;
using System;

class TutorialE : NextLevel, IQuest
{
    public string QuestID {get; set;}
    DialogueTrigger dialogueNode;


    public override void _Ready()
    {
        base._Ready();
        QuestID = "MAIN_MISSION";
        dialogueNode = (DialogueTrigger)GetNode("DialogueTrigger");
    }


    public void CheckQuest() {
        Godot.Collections.Array qArr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)MainNode.WorldSaveFile.Get("Quests"))[QuestID];
        if(qArr.Count == 4) {
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            dialogueNode.QueueFree();
        }
    }


    public void UpdateQuest() {}


}
