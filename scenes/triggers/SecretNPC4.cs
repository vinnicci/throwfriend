using Godot;
using System;
using System.Text;

public class SecretNPC4 : DialogueTrigger
{
    public override String ExecFunc(int currentDialogueSlot) {
        Main mainNode = (Main)GetNode("/root/Main");
        if(IsInstanceValid(mainNode.WorldSaveFile) == false) {
            base.ExecFunc(currentDialogueSlot);
        }
        Godot.Collections.Array arr =
        (Godot.Collections.Array)((Godot.Collections.Dictionary)mainNode.WorldSaveFile.Get("Quests"))["SECRET"];
        StringBuilder output = new StringBuilder();
        for(int i = 0; i <= 5; i++) {
            output.Append(arr[i] + " ");
        }
        return output.ToString();
    }
    

}
