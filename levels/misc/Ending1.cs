using Godot;
using System;

public class Ending1 : Level
{
    TextureRect cg;
    DialogueTrigger dialogue;
    [Export] Godot.Collections.Array<Texture> cgs;


    public override void _Ready()
    {
        base._Ready();
        cg = (TextureRect)GetNode("CanvasLayer/TextureRect");
        dialogue = (DialogueTrigger)GetNode("Objects/DialogueTrigger");
        cg.Texture = (Texture)cgs[0];
        if(dialogue.IsConnected(nameof(DialogueTrigger.NextDialogue), this, nameof(NextCG)) == false) {
            dialogue.Connect(nameof(DialogueTrigger.NextDialogue), this, nameof(NextCG));
        }
    }


    void NextCG(int currentDialogueSlot, Label text) {
        PlayerNode.IsStopped = true;
        if(currentDialogueSlot == 2) {
            cg.Texture = (Texture)cgs[1];
        }
        else if(currentDialogueSlot == 3) {
            cg.Texture = (Texture)cgs[2];
        }
        else if(currentDialogueSlot == 4) {
            dialogue.QueueFree();
            PlayerNode.Visible = false;
            ((Control)PlayerNode.GetNode("CanvasLayer/HotkeyHUD")).Visible = false;
            anim.Play("credits");
        }
    }


    void OnAnimFinished(String animName) {
        if(animName == "credits") {
            mainNode.GoToMainMenu();
        }
    }




}
