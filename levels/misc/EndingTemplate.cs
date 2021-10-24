using Godot;
using System;

public class EndingTemplate : Level
{
    protected TextureRect cg;
    [Export] protected Godot.Collections.Array<Texture> cgs;

    protected DialogueTrigger dialogue;


    public override void _Ready()
    {
        base._Ready();
        cg = (TextureRect)GetNode("CanvasLayer/TextureRect");
        dialogue = (DialogueTrigger)GetNode("Objects/DialogueTrigger");
        if(dialogue.IsConnected(nameof(DialogueTrigger.NextDialogue), this, nameof(NextCG)) == false) {
            dialogue.Connect(nameof(DialogueTrigger.NextDialogue), this, nameof(NextCG));
        }
        NextCG(0, default);
    }


    public async virtual void NextCG(int currentDialogueSlot, Label text) {
        PlayerNode.IsStopped = true;
        if(currentDialogueSlot == 0) {
            anim.Play("fade");
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            cg.Texture = (Texture)cgs[0];
        }
    }


    protected void ShowCredits() {
        dialogue.QueueFree();
        PlayerNode.Visible = false;
        ((Control)PlayerNode.GetNode("CanvasLayer/HotkeyHUD")).Visible = false;
        anim.Play("credits");
    }


    void OnAnimFinished(String animName) {
        if(animName == "credits") {
            mainNode.GoToMainMenu();
        }
    }




}
