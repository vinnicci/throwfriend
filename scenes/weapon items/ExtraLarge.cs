using Godot;
using System;

public class ExtraLarge : WeaponItem
{
    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("ExtraLarge");
    }


    [Export] PackedScene weapLarge;


    public override void InitEffect()
    {
        base.InitEffect();
        if(WeaponNode.Filename == Global.WEAP_LARGE_SCN) {
            return;
        }
        PlayerNode = WeaponNode.PlayerNode;
        Weapon largeWeap = (Weapon)weapLarge.Instance();
        if(WeaponNode.IsClone == false && PlayerNode.WeaponNode != largeWeap) {
            largeWeap.Damage *= 2;
            PlayerNode.UpdateWeapon(largeWeap);
        }
        if(IsInstanceValid(WeaponNode.Item1)) {
            largeWeap.ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            largeWeap.ActivateItem(1);
        }
        if(IsInstanceValid(WeaponNode.Item2)) {
            largeWeap.ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            largeWeap.ActivateItem(2);
        }
    }


}
