using Godot;
using System;

public class ItemSlot : TextureButton
{
    private Position2D iconPos;


    public override void _Ready()
    {
        base._Ready();
        iconPos = (Position2D)GetNode("IconSlot");
    }


    public void UpdateIcon(Item item) {
        if(iconPos.GetChildren().Count != 0) {
            iconPos.GetChild(0).QueueFree();
        }
        if(IsInstanceValid(item) == true) {
            Sprite icon = (Sprite)item.GetNode("Icon").Duplicate();
            iconPos.AddChild(icon);
            icon.Visible = true;
        }
    }


}
