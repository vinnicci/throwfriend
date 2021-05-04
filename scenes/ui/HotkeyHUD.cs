using Godot;
using System;

public class HotkeyHUD : Control
{
    private HKHudSlot slot1;
    private HKHudSlot slot2;
    private HKHudSlot slot3;
    private HKHudSlot slot4;


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
