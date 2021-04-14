using Godot;
using System;

public class ExtraLarge : WeaponItem
{
    private Weapon largeWeap;
    private Weapon tempWeap;
    public Player Player {get; set;}


    public override void _Ready()
    {
    }


    public override void InitEffect()
    {
        base.InitEffect();
        Player = WeaponNode.PlayerNode;
        PackedScene res = (PackedScene)ResourceLoader.Load("res://scenes/WeaponLarge.tscn");
        largeWeap = (Weapon)res.Instance();
        if(WeaponNode.IsClone == false && Player.WeaponNode != largeWeap) {
            tempWeap = Player.WeaponNode;
            Position2D weapSlot = (Position2D)tempWeap.GetParent();
            weapSlot.RemoveChild(tempWeap);
            weapSlot.AddChild(largeWeap);
            Player.WeaponNode = largeWeap;
            largeWeap.Connect("PickedUp", Player, "PickUpWeapon");
        }
        largeWeap.PlayerNode = Player;
        largeWeap.LevelNode = WeaponNode.LevelNode;
        largeWeap.RefreshItems();
        if(IsInstanceValid(WeaponNode.Item1) == true && WeaponNode.Item1 is ExtraLarge == false) {
            largeWeap.ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
        }
        if(IsInstanceValid(WeaponNode.Item2) == true && WeaponNode.Item2 is ExtraLarge == false) {
            largeWeap.ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
        }
        largeWeap.RefreshItems();
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        Position2D weapSlot = (Position2D)largeWeap.GetParent();
        weapSlot.RemoveChild(largeWeap);
        largeWeap.QueueFree();
        weapSlot.AddChild(tempWeap);
        Player.WeaponNode = tempWeap;
    }


}
