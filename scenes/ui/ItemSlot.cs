using Godot;
using System;

public class ItemSlot : Control
{
    public Item Item {get; set;}
    public Label DescriptionLabel {get; set;}
    public Loadout LoadoutNode {get; set;}

    Position2D iconPos;
    Label upgradeLabel;


    public override void _Ready()
    {
        base._Ready();
        iconPos = (Position2D)GetNode("IconSlot");
        upgradeLabel = (Label)GetNode("UpgradeLabel");
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


    void OnTextureButtonPressed() {
        if(upgradeLabel.Visible == true) {
            LoadoutNode.ShowItemSelection(GetParent().Name, Name, upgradeLabel);
        }
    }


    void OnTextureButtonMouseEntered() {
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = Item.itemDescription;
    }


    void OnTextureButtonMouseExited() {
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = "";
    }


    void OnTextureButtonHide() {
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = "";
    }


}
