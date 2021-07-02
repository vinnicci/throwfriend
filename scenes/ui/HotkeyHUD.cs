using Godot;
using System;

public class HotkeyHUD : Control
{
    HKHudSlot slot1;
    HKHudSlot slot2;
    HKHudSlot slot3;
    HKHudSlot slot4;


    public override void _Ready()
    {
        base._Ready();
        for(int i = 1; i <= 4; i++) {
            Set("slot" + i, (HKHudSlot)GetNode("Panel/HKHudSlot" + i));
        }
    }


    public void UpdateSlotIcon(int slotNum, Item item) {
        HKHudSlot slot = (HKHudSlot)Get("slot" + slotNum);
        slot.UpdateIcon(item);
    }


}
