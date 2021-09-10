using Godot;
using System;
using System.Collections.Generic;


///<summary>
/// To activate:
///     1. RefreshItems() (Player or Snark)
///     2. Add Item as child
///     3. Activate()
///</summary>
public abstract class Item : Node2D
{
    public List<String> incompatibilityList = new List<string>();
    [Export] public String itemDescription = "";
    protected Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            if(this is PlayerItem) {
                InitEffect();
            }
        }
    }
    protected Weapon weaponNode;
    public Weapon WeaponNode {
        get {
            return weaponNode;
        }
        set{
            weaponNode = value;
            if(this is WeaponItem) {
                InitEffect();
            }
        }
    }
    public Timer Cooldown {get; protected set;}
    public bool Active {get; protected set;}


    public override void _Ready()
    {
        base._Ready();
        Cooldown = (Timer)GetNode("Cooldown");
    }


    [Signal] public delegate void Activated();


    //intialization
    public virtual void InitEffect() {
        if(Active == true) {
            Switch(true, Active);
        }
    }


    //this func is called when this item hotkey is pressed
    public virtual void ApplyEffect() {}


    //used for switchable abilities
    public virtual void Switch(bool thisInst, bool state) {
        if(Active == false) {
            Active = true;
            EmitSignal(nameof(Activated), -1);
        }
        else {
            Active = false;
            EmitSignal(nameof(Activated), 0);
        }
    }


}
