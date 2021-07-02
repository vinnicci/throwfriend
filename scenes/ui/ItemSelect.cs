using Godot;
using System;

public class ItemSelect : Control
{
    [Export] PackedScene itemEx;
    public Label DescriptionNode {get; set;}
    public Loadout LoadoutNode {get; set;}

    String itemDescription;


    public override void _Ready()
    {
        base._Ready();
        Item item = (Item)itemEx.Instance();
        itemDescription = item.itemDescription;
        item.QueueFree();
    }


    void OnTextureButtonMouseEntered() {
        DescriptionNode.Text = itemDescription;
    }


    void OnTextureButtonMouseExited() {
        DescriptionNode.Text = "";
    }


    void OnTextureButtonPressed() {
        LoadoutNode.SelectItem((Item)itemEx.Instance());
    }


    void OnTextureButtonHide() {
        DescriptionNode.Text = "";
    }


}
