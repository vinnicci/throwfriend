using Godot;
using System;

public class SplitToThree : WeaponItem
{
    public Weapon[] Weaps {get; private set;}



    public override void _Notification(int what)
    {
        base._Notification(what);
        if(what == NotificationInstanced) {
            Weaps = new Weapon[2];
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(IsInstanceValid(WeaponNode) == false) {
            return;
        }
        else if(WeaponNode.CurrentState == Weapon.States.ACTIVE && Weaps[0].CurrentState == Weapon.States.HELD) {
            Split();
        }
        else if(WeaponNode.CurrentState == Weapon.States.HELD && Weaps[0].CurrentState != Weapon.States.HELD) {
            Weaps[0].OnPickedUp();
            Weaps[1].OnPickedUp();
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        PlayerNode = WeaponNode.PlayerNode;
        if(PlayerNode.IsConnected(nameof(Player.ActivatedWeaponItem), this, nameof(ApplyEffectsToClones)) == false) {
            PlayerNode.Connect(nameof(Player.ActivatedWeaponItem), this, nameof(ApplyEffectsToClones));
        }
        if(IsInstanceValid(Weaps[0]) == false) {
            PackedScene res = (PackedScene)ResourceLoader.Load(WeaponNode.Filename);
            Weaps[0] = (Weapon)res.Instance();
            Weaps[0].PlayerNode = PlayerNode;
            Weaps[0].IsClone = true;
            Weaps[1] = (Weapon)res.Instance();
            Weaps[1].PlayerNode = PlayerNode;
            Weaps[1].IsClone = true;
        }
        PlayerNode.AddChild(Weaps[0]);
        PlayerNode.AddChild(Weaps[1]);
        if(IsInstanceValid(WeaponNode.Item1) && WeaponNode.Item1 is SplitToThree == false) {
            Weaps[0].ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            Weaps[1].ItemSlot1Node.AddChild(WeaponNode.Item1.Duplicate());
            Weaps[0].ActivateItem(1);
            Weaps[1].ActivateItem(1);
        }
        if(IsInstanceValid(WeaponNode.Item2) && WeaponNode.Item2 is SplitToThree == false) {
            Weaps[0].ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            Weaps[1].ItemSlot2Node.AddChild(WeaponNode.Item2.Duplicate());
            Weaps[0].ActivateItem(2);
            Weaps[1].ActivateItem(2);
        }
        Weaps[0].GetParent().RemoveChild(Weaps[0]);
        Weaps[1].GetParent().RemoveChild(Weaps[1]);
    }


    void ApplyEffectsToClones(int num) {
        WeaponItem weap0Item = (WeaponItem)Weaps[0].Get("Item" + num);
        WeaponItem weap1Item = (WeaponItem)Weaps[1].Get("Item" + num);
        if(IsInstanceValid(weap0Item)) {
            if(WeaponNode.CurrentState == Weapon.States.HELD) {
                weap0Item.Switch(true, weap0Item.Active == false);
                weap1Item.Switch(true, weap1Item.Active == false);
            }
            else {
                weap0Item.ApplyEffect();
                weap1Item.ApplyEffect();
            }
        }
    }


    void Split() {
        int strength = PlayerNode.ThrowStrength;
        Weaps[0].Throw(strength, WeaponNode.GlobalPosition, Vector2.Zero,
        Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees + (float)GD.RandRange(0,30)));
        Weaps[1].Throw(strength, WeaponNode.GlobalPosition, Vector2.Zero,
        Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees - (float)GD.RandRange(0,30)));
    }


}
