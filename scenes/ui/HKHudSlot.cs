using Godot;
using System;

public class HKHudSlot : ColorRect
{
    public Item Item {get; set;}

    Position2D iconPos;
    AnimationPlayer anim;
    Timer cooldown;


    public override void _Ready()
    {
        base._Ready();
        iconPos = (Position2D)GetNode("IconSlot");
        anim = (AnimationPlayer)GetNode("Anim");
        cooldown = (Timer)GetNode("Cooldown");
    }


    public void UpdateIcon(Item item) {
        if(IsInstanceValid(Item)) {
            iconPos.GetChild(0).QueueFree();
        }
        Item = item;
        if(IsInstanceValid(Item)) {
            Sprite icon = (Sprite)Item.GetNode("Icon").Duplicate();
            iconPos.AddChild(icon);
            icon.Visible = true;
            if(Item.IsConnected(nameof(Item.Activated), this, nameof(AnimateCooldown)) == false) {
                Godot.Collections.Array arr = new Godot.Collections.Array();
                arr.Add(Item.Cooldown.WaitTime);
                Item.Connect(nameof(Item.Activated), this, nameof(AnimateCooldown), arr);
            }
        }
    }


    Color RED = new Color(1,0,0);


    void AnimateCooldown(float length) {
        cooldown.Start(length - 0.2f);
        Color = RED;
    }


    void OnCooldownTimeout() {
        anim.Play("cooldown_flash");
    }


}
