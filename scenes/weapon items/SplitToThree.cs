using Godot;
using System;

public class SplitToThree : WeaponItem
{
    private Weapon[] weaps = new Weapon[2];
    public Player Player {get; set;}


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(WeaponNode.CurrentState == Weapon.States.ACTIVE && weaps[0].CurrentState == Weapon.States.HELD &&
        weaps[1].CurrentState == Weapon.States.HELD) {
            Split();
        }
        if(weaps[0].CurrentState != Weapon.States.HELD && WeaponNode.CurrentState == Weapon.States.HELD) {
            weaps[0].PickUp();
        }
        if(weaps[1].CurrentState != Weapon.States.HELD && WeaponNode.CurrentState == Weapon.States.HELD) {
            weaps[1].PickUp();
        }
    }


    public override void InitEffect()
    {
        base.InitEffect();
        Player = WeaponNode.PlayerNode;
        PackedScene res = (PackedScene)ResourceLoader.Load(WeaponNode.Filename);
        weaps[0] = (Weapon)res.Instance();
        weaps[1] = (Weapon)res.Instance();
        weaps[0].RefreshItems();
        weaps[1].RefreshItems();
        weaps[0].PlayerNode = Player;
        weaps[0].LevelNode = WeaponNode.LevelNode;
        weaps[0].IsClone = true;
        weaps[1].PlayerNode = Player;
        weaps[1].LevelNode = WeaponNode.LevelNode;
        weaps[1].IsClone = true;
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
        weaps[0].RefreshItems();
        weaps[1].RefreshItems();
    }


    private void Split() {
        int strength = Player.ThrowStrength;
        weaps[0].Throw(strength, WeaponNode.GlobalPosition, Godot.Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees +
        (float)GD.RandRange(0,30)));
        weaps[1].Throw(strength, WeaponNode.GlobalPosition, Godot.Mathf.Deg2Rad(WeaponNode.GlobalRotationDegrees -
        (float)GD.RandRange(0,30)));
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
        weaps[0].GetParent().RemoveChild(weaps[0]);
        weaps[1].GetParent().RemoveChild(weaps[1]);
        weaps[0].QueueFree();
        weaps[1].QueueFree();
    }


}
