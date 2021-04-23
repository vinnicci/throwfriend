using Godot;
using System;

public class WeaponItem : Item
{
    protected Weapon weaponNode;
    public Weapon WeaponNode {
        get {
            return weaponNode;
        }
        set {
            weaponNode = value;
            InitEffect();
        }
    }


}
