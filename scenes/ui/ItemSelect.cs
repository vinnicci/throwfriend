using Godot;
using System;

public class ItemSelect : Control
{
    [Export] PackedScene itemEx;
    public Label DescriptionNode {get; set;}
    public Loadout LoadoutNode {get; set;}

    private String itemDescription;


    public override void _Ready()
    {
        base._Ready();
        Item item = (Item)itemEx.Instance();
        itemDescription = item.itemDescription;
        item.QueueFree();
    }


    private void OnTextureButtonMouseEntered() {
        DescriptionNode.Text = itemDescription;
    }


    private void OnTextureButtonMouseExited() {
        DescriptionNode.Text = "";
    }


    private void OnTextureButtonPressed() {
        LoadoutNode.SelectItem((Item)itemEx.Instance());
    }


    private void OnTextureButtonHide() {
        DescriptionNode.Text = "";
    }


}
