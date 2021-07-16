using Godot;
using System;

public class SplitToThree : WeaponItem
{
    Weapon[] weaps = new Weapon[2];


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(WeaponNode.CurrentState == Weapon.States.ACTIVE && weaps[0].CurrentState == Weapon.States.HELD &&
        weaps[1].CurrentState == Weapon.States.HELD) {
            Split();
        }
        if(weaps[0].CurrentState != Weapon.States.HELD && WeaponNode.CurrentState == Weapon.States.HELD) {
            weaps[0].OnPickedUp();
        }
        if(weaps[1].CurrentState != Weapon.States.HELD && WeaponNode.CurrentState == Weapon.States.HELD) {
            weaps[1].OnPickedUp();
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode = WeaponNode.PlayerNode;
        if(PlayerNode.IsConnected(nameof(Player.ActivatedWeaponItem), this, nameof(ApplyEffectsToClones)) == false) {
            PlayerNode.Connect(nameof(Player.ActivatedWeaponItem), this, nameof(ApplyEffectsToClones));
        }
        PackedScene res = (PackedScene)ResourceLoader.Load(WeaponNode.Filename);
        weaps[0] = (Weapon)res.Instance();
        weaps[1] = (Weapon)res.Instance();
        weaps[0].RefreshItems();
        weaps[1].RefreshItems();
        weaps[0].PlayerNode = PlayerNode;
        weaps[0].IsClone = true;
        weaps[0].Damage = WeaponNode.Damage;
        weaps[1].PlayerNode = PlayerNode;
        weaps[1].IsClone = true;
        weaps[1].Damage = WeaponNode.Damage;
        if(IsInstanceValid(WeaponNode.Item1) == true && WeaponNode.Item1 is SplitToThree == false) {
            weaps[0].ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            weaps[1].ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            weaps[0].RefreshItems();
            weaps[1].RefreshItems();
            weaps[0].ActivateItem(1);
            weaps[1].ActivateItem(1);
        }
        if(IsInstanceValid(WeaponNode.Item2) == true && WeaponNode.Item2 is SplitToThree == false) {
            weaps[0].ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            weaps[1].ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            weaps[0].RefreshItems();
            weaps[1].RefreshItems();
            weaps[0].ActivateItem(2);
            weaps[1].ActivateItem(2);
        }
    }


    void ApplyEffectsToClones(int num) {
        WeaponItem weap0Item = (WeaponItem)weaps[0].Get("Item" + num);
        WeaponItem weap1Item = (WeaponItem)weaps[1].Get("Item" + num);
        if((IsInstanceValid(weap0Item) && IsInstanceValid(weap1Item)) == true) {
            weap0Item.ApplyEffect();
            weap1Item.ApplyEffect();
        }
    }


    void Split() {
        int strength = PlayerNode.ThrowStrength;
        weaps[0].Throw(strength, WeaponNode.GlobalPosition, Vector2.Zero,
        Godot.Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees + (float)GD.RandRange(0,30)));
        weaps[1].Throw(strength, WeaponNode.GlobalPosition, Vector2.Zero,
        Godot.Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees - (float)GD.RandRange(0,30)));
    }


}
