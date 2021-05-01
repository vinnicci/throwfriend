using Godot;
using System;

public class Loadout : Control
{
    public Player PlayerNode {get; set;}
    public Weapon WeaponNode {get; set;}
    public ItemSlot Slot1 {get; private set;}
    public ItemSlot Slot2 {get; private set;}
    public ItemSlot Slot3 {get; private set;}
    public ItemSlot Slot4 {get; private set;}

    private Button slot1KeyBind;
    private Button slot2KeyBind;
    private Button slot3KeyBind;
    private Button slot4KeyBind;


    public override void _Ready()
    {
        base._Ready();
        Slot1 = (ItemSlot)GetNode("PlayerPanel/ItemSlot");
        Slot2 = (ItemSlot)GetNode("PlayerPanel/ItemSlot2");
        Slot3 = (ItemSlot)GetNode("WeaponPanel/ItemSlot");
        Slot4 = (ItemSlot)GetNode("WeaponPanel/ItemSlot2");
        for(int i = 1; i <= 4; i++) {
            Godot.Collections.Array arr = new Godot.Collections.Array(i);
            String currentKeyBind = "slot" + i + "KeyBind";
            ItemSlot slot = (ItemSlot)Get("Slot" + i);
            Set(currentKeyBind, slot.GetNode("Button"));
            Button keyBind = (Button)Get(currentKeyBind);
            keyBind.Connect("pressed", this, "OnKeyBindPressed", arr);
        }
    }

    
    public void UpdateSlotIcon(int slotNum) {
        switch(slotNum) {
            case 1: Slot1.UpdateIcon(PlayerNode.Item1); break;
            case 2: Slot2.UpdateIcon(PlayerNode.Item2); break;
            case 3: Slot3.UpdateIcon(WeaponNode.Item1); break;
            case 4: Slot4.UpdateIcon(WeaponNode.Item2); break;
            default: GD.PrintErr("Slot doesn't exist."); break;
        }
    }


    private void OnKeyBindPressed(int slotNum) {
        switch(slotNum) {
            case 1: GD.Print("slot" + slotNum + " pressed"); break;
            case 2: GD.Print("slot" + slotNum + " pressed"); break;
            case 3: GD.Print("slot" + slotNum + " pressed"); break;
            case 4: GD.Print("slot" + slotNum + " pressed"); break;
            default: GD.PrintErr("Non existing slot pressed on Loadout Node."); break;
        }
    }


}
