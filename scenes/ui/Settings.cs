using Godot;
using System;

public class Settings : Control
{
    public AnimationPlayer InGameUIAnim {get; set;}


    public override void _Ready()
    {
        base._Ready();
        Panel panel = (Panel)GetNode("Panel");
        for(int i = 0; i <= panel.GetChildren().Count-1; i++) {
            Node obj = panel.GetChild(i);
            Button button;
            Godot.Collections.Array arr;
            if(obj is Label && obj.IsInGroup("Key") == true) {
                button = (Button)obj.GetChild(0);
            }
            else if(obj is Button) {
                button = (Button)obj;
            }
            else {
                continue;
            }
            arr = new Godot.Collections.Array();
            arr.Add(obj.Name);
            button.Connect("pressed", this, "OnButtonPressed", arr);
        }
    }


    private void OnButtonPressed(String slotName) {
        switch(slotName) {
            case "Up": {
                GD.Print("up pressed");
            }
            break;
            case "Down": {
                GD.Print("down pressed");
            }
            break;
            case "Left": {
                GD.Print("left pressed");
            }
            break;
            case "Right": {
                GD.Print("right pressed");
            }
            break;
            case "Throw": {
                GD.Print("throw pressed");
            }
            break;
            case "Menu": {
                GD.Print("menu pressed");
            }
            break;
            case "Back": {
                if(InGameUIAnim.IsPlaying() == false) {
                    InGameUIAnim.Play("settings_exit");
                }
            }
            break;
            default: {
                GD.PrintErr("Wrong object reference in Settings Node.");
            }
            break;
        }
    }


}
