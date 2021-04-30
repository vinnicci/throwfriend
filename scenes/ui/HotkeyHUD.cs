using Godot;
using System;

public class HotkeyHUD : Control
{
    private Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            UpdateSlotIcon(1);
            UpdateSlotIcon(2);
        }
    }
    private Weapon weaponNode;
    public Weapon WeaponNode {
        get {
            return weaponNode;
        }
        set{
            weaponNode = value;
            UpdateSlotIcon(3);
            UpdateSlotIcon(4);
        }
    }

    private HKHudSlot slot1;
    private HKHudSlot slot2;
    private HKHudSlot slot3;
    private HKHudSlot slot4;


    public override void _Ready()
    {
        base._Ready();
        slot1 = (HKHudSlot)GetNode("Panel/HKHudSlot1");
        slot2 = (HKHudSlot)GetNode("Panel/HKHudSlot2");
        slot3 = (HKHudSlot)GetNode("Panel/HKHudSlot3");
        slot4 = (HKHudSlot)GetNode("Panel/HKHudSlot4");
    }


    private void UpdateSlotIcon(int slotNum) {
        switch(slotNum) {
            case 1: slot1.UpdateIcon(PlayerNode.Item1); break;
            case 2: slot2.UpdateIcon(PlayerNode.Item2); break;
            case 3: slot3.UpdateIcon(WeaponNode.Item1); break;
            case 4: slot4.UpdateIcon(WeaponNode.Item2); break;
            default: GD.PrintErr("Slot doesn't exist."); break;
        }
    }


}
