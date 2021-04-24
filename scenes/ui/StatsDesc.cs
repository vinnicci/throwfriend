using Godot;
using System;

public class StatsDesc : Control
{
    public Label DescriptionLabel {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        DescriptionLabel = (Label)GetNode("DescPanel/Description");
    }



}
