using Godot;
using System;

public class Weapon : RigidBody2D
{
    public Level LevelNode {get; set;}
    public bool IsTakable {get; private set;}
    public int Damage {get; set;}
    public WeaponItem ItemSlot1 {get; set;}
    public WeaponItem ItemSlot2 {get; set;}


    public override void _Ready()
    {
        // PackedScene itemPack = (PackedScene)ResourceLoader.Load("res://scenes/weapon items/ExtraDamage.tscn");
        // ItemSlot1 = (WeaponItem)itemPack.Instance();

        IsTakable = true;
        Damage = 1;
        if(ItemSlot1 != null) {
            ItemSlot1.WeaponNode = this;
        }
        if(ItemSlot2 != null) {
            ItemSlot2.WeaponNode = this;
        }
    }


    // public override void _Process(float delta)
    // {
    // }


    private const int WEAP_MIN_LIN_VEL_LEN = 350;
    // private const short BIT_MASK_CHAR = 0;
    // private const short BIT_MASK_LVL = 1;
    // private const short BIT_MASK_PLAYER = 2;


    public override void _PhysicsProcess(float delta)
    {
        if(IsTakable == false && LinearVelocity.Length() <= WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_CHAR, false);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            IsTakable = true;
        }
    }


    public void Throw(int throwStrength, Vector2 pos, float rotation) {
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(Global.BIT_MASK_CHAR, true);
        SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
        GetParent().RemoveChild(this);
        LevelNode.AddChild(this);
        GlobalPosition = pos;
        GlobalRotation = rotation;
        Vector2 throwVector = new Vector2(throwStrength, 0);
        ApplyCentralImpulse(throwVector.Rotated(rotation));
        IsTakable = false;
    }


    [Signal] public delegate void PickedUp();
    private Godot.Collections.Array hitList = new Godot.Collections.Array();
    private const int KNOCKBACK = 150;


    private void OnWeaponBodyEntered(Godot.Object body) {
        if(body is Player && IsTakable == true) {
            SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            CallDeferred("PickUpDeferred");
        }
        else if(body is Enemy && IsTakable == false && hitList.Contains(body) == false) {
            Enemy enemy = (Enemy)body;
            enemy.Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), Damage);
            hitList.Add(enemy);
        }
        int i = 1;
        if(GD.RandRange(0, 1.0) <= 0.5) {
            i *= -1;
        }
        AngularVelocity = i * 50;
    }


    private void PickUpDeferred() {
        GetParent().RemoveChild(this);
        EmitSignal("PickedUp");
    }


}
