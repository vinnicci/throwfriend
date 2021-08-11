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


    public void ShowDialogue(Texture portrait, String name, Godot.Collections.Array stringArr) {
        anim.Queue("show_dialogue");
        dialoguePortrait.Texture = portrait;
        dialogueSpeaker.Text = name;
        dialogueArr = stringArr;
        currentDialogueSlot = 0;
        NextDialogue();
    }


    Godot.Collections.Array dialogueArr;
    int currentDialogueSlot;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(Input.IsActionJustPressed("ui_accept") && Visible) {
            NextDialogue();
        }
    }


    const float TEXT_SPEED = 50f;


    void NextDialogue() {
        if(currentDialogueSlot > dialogueArr.Count - 1) {
            currentDialogueSlot = 0;
        }
        dialogueText.PercentVisible = 0;
        dialogueText.Text = (String)dialogueArr[currentDialogueSlot];
        tween.StopAll();
        tween.InterpolateProperty(dialogueText, "percent_visible",
            0, 1f, dialogueText.Text.Length/TEXT_SPEED, Tween.TransitionType.Linear);
        tween.Start();
        currentDialogueSlot += 1;
    }


    public void HideDialogue() {
        anim.Queue("hide_dialogue");
    }


}
