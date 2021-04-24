using Godot;
using System;

public class InGame : Control
{
    public Loadout LoadoutNode {get; set;}
    public StatsDesc StatsDescNode {get; set;}
    private Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            LoadoutNode.PlayerNode = PlayerNode;
            LoadoutNode.UpdateSlotIcon(1);
            LoadoutNode.UpdateSlotIcon(2);
        }
    }
    private Weapon weaponNode;
    public Weapon WeaponNode {
        get {
            return weaponNode;
        }
        set {
            weaponNode = value;
            LoadoutNode.WeaponNode = WeaponNode;
            LoadoutNode.UpdateSlotIcon(3);
            LoadoutNode.UpdateSlotIcon(4);
        }
    }


    public override void _Ready()
    {
        base._Ready();
        LoadoutNode = (Loadout)GetNode("Loadout");
        StatsDescNode = (StatsDesc)GetNode("StatsDesc");
        for(int i = 1; i <= 4; i++) {
            ItemSlot slot = (ItemSlot)LoadoutNode.Get("Slot" + i.ToString());
            slot.DescriptionLabel = StatsDescNode.DescriptionLabel;
        }
    }


}
