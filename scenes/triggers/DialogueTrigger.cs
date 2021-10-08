using Godot;
using System;

public class DialogueTrigger : Area2D
{
    [Export] Texture portrait;
    [Export] String name = "";
    [Export] Godot.Collections.Array stringArr = new Godot.Collections.Array();
    [Export] float portraitScale = 1f;


    void OnDialogueTriggerBodyEntered(Godot.Object body) {
        if(body is Player) {
            ((Player)body).TriggerDialogue(portrait, name, stringArr, portraitScale, this, true);
        }
    }


    void OnDialogueTriggerBodyExited(Godot.Object body) {
        if(body is Player) {
            ((Player)body).TriggerDialogue(default, default, default, default);
        }
    }


    public virtual String ExecFunc(int currentDialogueSlot) {
        return default;
    }


}
