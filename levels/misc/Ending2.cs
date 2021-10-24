using Godot;
using System;

public class Ending2 : EndingTemplate
{
    public async override void NextCG(int currentDialogueSlot, Label text) {
        base.NextCG(currentDialogueSlot, text);
        if(currentDialogueSlot == 2) {
            anim.Play("fade");
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            cg.Texture = (Texture)cgs[1];
        }
        else if(currentDialogueSlot == 4) {
            ShowCredits();
        }
    }


}
