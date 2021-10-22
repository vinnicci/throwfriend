using Godot;
using System;
using System.Text;

public class NPC4 : DialogueTrigger
{
    public override void NextDialogueEvent(int currentDialogueSlot, Label text) {
        //triggers on dialogue slot 7
        if(currentDialogueSlot != 7) {
            return;
        }
        Main mainNode = (Main)GetNode("/root/Main");
        if(IsInstanceValid(mainNode.WorldSaveFile) == false) {
            return;
        }
         Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("Quests"))["SECRET"];
        StringBuilder output = new StringBuilder();
        for(int i = 0; i <= 5; i++) {
            output.Append(arr[i] + " ");
        }
        text.Text = output.ToString();
    }


}
