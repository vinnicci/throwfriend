using Godot;
using System;
using System.Collections.Generic;


///<summary>
/// To activate:
///     1. RefreshItems() (Player or Snark)
///     2. Add Item as child
///     3. Activate()
///</summary>
public abstract class Item : Node2D, ISoundEmitter
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
            weaponNode = playerNode.WeaponNode;
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
            playerNode = weaponNode.PlayerNode;
            if(this is WeaponItem) {
                InitEffect();
            }
        }
    }
    public Timer Cooldown {get; protected set;}
    public bool Active {get; protected set;}
    public Node2D SoundsNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        Cooldown = (Timer)GetNode("Cooldown");
        SoundsNode = (Node2D)GetNode("Sounds");
        SoundsDict = new Godot.Collections.Dictionary<String, AudioStreamPlayer2D>();
    }


    [Signal] public delegate void Activated();


    //intialization
    public virtual void InitEffect() {
        if(Active) {
            Switch(true, Active == false);
        }
    }


    //this func is called when this item hotkey is pressed
    public virtual void ApplyEffect() {}


    //used for switchable abilities
    public virtual void Switch(bool thisInst, bool active) {
        if(Active == false) {
            EmitSignal(nameof(Activated), -1);
        }
        else {
            EmitSignal(nameof(Activated), 0);
        }
        Active = active;
    }


    public Godot.Collections.Dictionary<String, AudioStreamPlayer2D> SoundsDict {get; set;}


    public void PlaySoundEffect(string soundName) {
        if(SoundsNode.HasNode(soundName) == false) {
            return;
        }
        if(SoundsDict.ContainsKey(soundName) == false) {
            SoundsDict.Add(soundName, (AudioStreamPlayer2D)SoundsNode.GetNode(soundName));
        }
        AudioStreamPlayer2D soundNode = SoundsDict[soundName];
        soundNode.Play();
    }


}
