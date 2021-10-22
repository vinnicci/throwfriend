using Godot;
using System;

public class HotkeyHUD : Control
{
    HKHudSlot slot1;
    HKHudSlot slot2;
    HKHudSlot slot3;
    HKHudSlot slot4;
    AnimationPlayer anim;
    Tween tween;
    Panel dialogue;
    Sprite dialoguePortrait;
    Label dialogueSpeaker;
    Label dialogueText;


    public override void _Ready()
    {
        base._Ready();
        anim = (AnimationPlayer)GetNode("Anim");
        tween = (Tween)GetNode("Tween");
        dialogue = (Panel)GetNode("DialoguePanel");
        dialoguePortrait = (Sprite)GetNode("DialoguePanel/Portrait");
        dialogueSpeaker = (Label)GetNode("DialoguePanel/Name");
        dialogueText = (Label)GetNode("DialoguePanel/Dialogue");
        for(int i = 1; i <= 4; i++) {
            Set("slot" + i, (HKHudSlot)GetNode("Panel/HKHudSlot" + i));
        }
    }


    public void UpdateSlotIcon(int slotNum, Item item) {
        HKHudSlot slot = (HKHudSlot)Get("slot" + slotNum);
        slot.UpdateIcon(item);
    }


    public void ShowDialogue(Texture portrait, String name, Godot.Collections.Array stringArr, float portraitScale,
    DialogueTrigger dialogueInst) {
        anim.Queue("show_dialogue");
        dialoguePortrait.Texture = portrait;
        dialoguePortrait.Scale = new Vector2(portraitScale, portraitScale);
        dialogueSpeaker.Text = name;
        dialogueArr = stringArr;
        currentDialogueSlot = 0;
        this.dialogueInst = dialogueInst;
        NextDialogue();
    }


    Godot.Collections.Array dialogueArr;
    DialogueTrigger dialogueInst;
    int currentDialogueSlot;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(Input.IsActionJustPressed("ui_accept") && dialogue.Visible) {
            NextDialogue();
        }
    }


    const float TEXT_SPEED = 50f;


    void NextDialogue() {
        if(currentDialogueSlot > dialogueArr.Count - 1) {
            currentDialogueSlot = 0;
        }
        dialogueText.PercentVisible = 0;
        // if((String)dialogueArr[currentDialogueSlot] == "EXEC_FUNC") {
        //     dialogueText.Text = dialogueInst.ExecFunc(currentDialogueSlot);
        // }
        // else {
        dialogueText.Text = (String)dialogueArr[currentDialogueSlot];
        // }
        tween.StopAll();
        tween.InterpolateProperty(dialogueText, "percent_visible",
            0, 1f, dialogueText.Text.Length/TEXT_SPEED, Tween.TransitionType.Linear);
        tween.Start();
        dialogueInst.EmitSignal(nameof(DialogueTrigger.NextDialogue), currentDialogueSlot, dialogueText);
        currentDialogueSlot += 1;
    }


    public void HideDialogue() {
        anim.Queue("hide_dialogue");
    }


}
