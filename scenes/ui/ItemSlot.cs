using Godot;
using System;

public class ItemSlot : Control
{
    private Position2D iconPos;
    public Item Item {get; set;}
    public Label DescriptionLabel {get; set;}


    public override void _Ready()
    {
        base._Ready();
        iconPos = (Position2D)GetNode("IconSlot");
    }


    public void UpdateIcon(Item item) {
        if(IsInstanceValid(Item) == true) {
            iconPos.GetChild(0).QueueFree();
        }
        Item = item;
        if(IsInstanceValid(Item) == true) {
            Sprite icon = (Sprite)Item.GetNode("Icon").Duplicate();
            iconPos.AddChild(icon);
            icon.Visible = true;
        }
    }


    private void OnTextureButtonPressed() {
        GD.Print("pressed");
    }


    private void OnTextureButtonMouseEntered() {
        GD.Print("hover");
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = Item.Name + "\n" + Item.itemDescription;
    }


    private void OnTextureButtonMouseExited() {
        GD.Print("hover exit");
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = "";
    }


    private void OnTextureButtonHide() {
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = "";
    }


}
