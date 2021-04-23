using Godot;
using System;

public class Loadout : Control
{
    public Player PlayerNode {get; set;}
    public Weapon WeaponNode {get; set;}
    private ItemSlot slot1;
    private ItemSlot slot2;
    private ItemSlot slot3;
    private ItemSlot slot4;


    public override void _Ready()
    {
        base._Ready();
        slot1 = (ItemSlot)GetNode("PlayerPanel/ItemSlot");
        slot2 = (ItemSlot)GetNode("PlayerPanel/ItemSlot2");
        slot3 = (ItemSlot)GetNode("WeaponPanel/ItemSlot");
        slot4 = (ItemSlot)GetNode("WeaponPanel/ItemSlot2");
    }

    
    public void UpdateSlotIcon(int slotNum) {
        switch(slotNum) {
            case 1: slot1.UpdateIcon(PlayerNode.Item1); break;
            case 2: slot2.UpdateIcon(PlayerNode.Item2); break;
            case 3: slot3.UpdateIcon(WeaponNode.Item1); break;
            case 4: slot4.UpdateIcon(WeaponNode.Item2); break;
            default: GD.PrintErr("Slot doesn't exist."); break;
        }
    }


}
