using Godot;
using System;

public class WarningText : Label
{
    private AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        anim = (AnimationPlayer)GetNode("Anim");
    }


    public void ShowWarning() {
        anim.Stop();
        anim.Play("show_warning");
    }


}
