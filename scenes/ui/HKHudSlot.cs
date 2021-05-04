using Godot;
using System;

public class HKHudSlot : ColorRect
{
    public Item Item {get; set;}

    private Position2D iconPos;
    private AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        iconPos = (Position2D)GetNode("IconSlot");
        anim = (AnimationPlayer)GetNode("Anim");
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
            if(Item.IsConnected("Activated", this, "AnimateCooldown") == false) {
                Item.Connect("Activated", this, "AnimateCooldown");
            }
        }
    }


    private void AnimateCooldown() {
        anim.Play("cooldown");
    }


}
