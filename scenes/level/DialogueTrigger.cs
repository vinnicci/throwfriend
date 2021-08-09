using Godot;
using System;

public class DialogueTrigger : Area2D
{
    [Export] Texture portrait;
    [Export] String name = "";
    [Export] Godot.Collections.Array stringArr = new Godot.Collections.Array();


    void OnDialogueTriggerBodyEntered(Godot.Object body) {
        if(body is Player) {
            ((Player)body).TriggerDialogue(portrait, name, stringArr, true);
        }
    }


    void OnDialogueTriggerBodyExited(Godot.Object body) {
        if(body is Player) {
            ((Player)body).TriggerDialogue(default, default, default, false);
        }
    }


}
