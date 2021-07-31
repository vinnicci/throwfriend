using Godot;
using System;

public class InGame : Control
{
    Loadout loadoutNode;
    StatsDesc statsDescNode;
    Settings settingsNode;
    AnimationPlayer inGameUIAnim;
    Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            statsDescNode.PlayerNode = PlayerNode;
            loadoutNode.PlayerNode = PlayerNode;
            loadoutNode.UpdateSlotIcon(1);
            loadoutNode.UpdateSlotIcon(2);
            loadoutNode.UpdateSlotIcon(3);
            loadoutNode.UpdateSlotIcon(4);
        }
    }
    // Weapon weaponNode;
    // public Weapon WeaponNode {
    //     get {
    //         return weaponNode;
    //     }
    //     set {
    //         weaponNode = value;
    //         loadoutNode.WeaponNode = WeaponNode;
    //     }
    // }


    public override void _Ready()
    {
        base._Ready();
        loadoutNode = (Loadout)GetNode("Loadout");
        statsDescNode = (StatsDesc)GetNode("StatsDesc");
        settingsNode = (Settings)GetNode("Settings");
        inGameUIAnim = (AnimationPlayer)GetNode("Anim");
        for(int i = 1; i <= 4; i++) {
            ItemSlot slot = (ItemSlot)loadoutNode.Get("Slot" + i.ToString());
            slot.DescriptionLabel = statsDescNode.DescriptionLabel;
        }
        statsDescNode.InGameUIAnim = inGameUIAnim;
        statsDescNode.SettingsNode = settingsNode;
        settingsNode.InGameUIAnim = inGameUIAnim;
    }


}
