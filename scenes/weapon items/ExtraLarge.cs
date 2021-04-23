using Godot;
using System;

public class ExtraLarge : WeaponItem
{
    public Player Player {get; set;}

    private Weapon largeWeap;
    private Weapon tempWeap;


    public override void _Ready()
    {
        incompatibilityList.Add("ExtraLarge");
    }


    [Export] PackedScene weapLarge;


    public override void InitEffect()
    {
        base.InitEffect();
        Player = WeaponNode.PlayerNode;
        largeWeap = (Weapon)weapLarge.Instance();
        if(WeaponNode.IsClone == false && Player.WeaponNode != largeWeap) {
            tempWeap = Player.WeaponNode;
            Position2D weapSlot = (Position2D)tempWeap.GetParent();
            weapSlot.RemoveChild(tempWeap);
            weapSlot.AddChild(largeWeap);
            Player.WeaponNode = largeWeap;
            largeWeap.Connect("PickedUp", Player, "PickUpWeapon");
        }
        largeWeap.RefreshItems();
        largeWeap.PlayerNode = Player;
        largeWeap.LevelNode = WeaponNode.LevelNode;
        if(IsInstanceValid(WeaponNode.Item1) == true) {
            largeWeap.ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            largeWeap.RefreshItems();
            if(WeaponNode.Item1 is ExtraLarge == false) {
                largeWeap.ActivateItem(1);
            }
        }
        if(IsInstanceValid(WeaponNode.Item2) == true) {
            largeWeap.ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            largeWeap.RefreshItems();
            if(WeaponNode.Item2 is ExtraLarge == false) {
                largeWeap.ActivateItem(2);
            }
        }
        largeWeap.RefreshItems();
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        Position2D weapSlot = (Position2D)largeWeap.GetParent();
        if(IsInstanceValid(largeWeap.Item1) == true) {
            largeWeap.Item1.GetParent().RemoveChild(largeWeap.Item1);
            tempWeap.ItemSlot1Node.AddChild(largeWeap.Item1);
        }
        if(IsInstanceValid(largeWeap.Item2) == true) {
            largeWeap.Item2.GetParent().RemoveChild(largeWeap.Item2);
            tempWeap.ItemSlot2Node.AddChild(largeWeap.Item2);
        }
        weapSlot.RemoveChild(largeWeap);
        weapSlot.AddChild(tempWeap);
        Player.WeaponNode = tempWeap;
        tempWeap.RefreshItems();
        largeWeap.QueueFree();
    }


}
