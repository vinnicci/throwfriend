using Godot;
using System;

public class ItemSlot : Control
{
    public Item Item {get; set;}
    public Label DescriptionLabel {get; set;}
    public Loadout LoadoutNode {get; set;}

    private Position2D iconPos;
    private Label upgradeLabel;


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


    private void OnTextureButtonPressed() {
        if(upgradeLabel.Visible == true) {
            LoadoutNode.ShowItemSelection(GetParent().Name, Name, upgradeLabel);
        }
    }


    private void OnTextureButtonMouseEntered() {
        if(IsInstanceValid(Item) == false) {
            return;
        }
        DescriptionLabel.Text = Item.itemDescription;
    }


    private void OnTextureButtonMouseExited() {
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
