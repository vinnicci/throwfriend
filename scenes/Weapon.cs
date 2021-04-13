using Godot;
using System;
using System.Collections.Generic;


public class Weapon : RigidBody2D
{
    public Level LevelNode {get; set;}
    public bool IsTakable {get; set;}
    public int Damage {get; set;}
    public Vector2 Velocity {get; set;}
    private Node2D itemSlot1Node;
    public WeaponItem Item1 {get; set;}
    private Node2D itemSlot2Node;
    public WeaponItem Item2 {get; set;}


    public override void _Ready()
    {
        IsTakable = true;
        Damage = 1;
        RefreshItems();
    }


    public void RefreshItems() {
        itemSlot1Node = (Node2D)GetNode("ItemSlot1");
        if(itemSlot1Node.GetChildCount() != 0) {
            Item1 = (WeaponItem)itemSlot1Node.GetChild(0);
            Item1.WeaponNode = this;
        }
        itemSlot2Node = (Node2D)GetNode("ItemSlot2");
        if(itemSlot2Node.GetChildCount() != 0) {
            Item2 = (WeaponItem)itemSlot2Node.GetChild(0);
            Item2.WeaponNode = this;
        }
    }


    // public override void _Process(float delta)
    // {
    // }


    private Vector2 teleportPos;


    public void Teleport(Vector2 global_pos) {
        teleportPos = global_pos;
    }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        //teleport
        if(teleportPos != Vector2.Zero) {
            Transform2D trans = new Transform2D(Rotation, teleportPos);
            state.Transform = trans;
            teleportPos = Vector2.Zero;
        }
        //velocity
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity);
        Velocity = Vector2.Zero;
    }


    private const int WEAP_MIN_LIN_VEL_LEN = 200;


    public override void _PhysicsProcess(float delta)
    {
        if(IsTakable == false && LinearVelocity.Length() <= WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_CHAR, false);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            IsTakable = true;
        }
    }


    public void Throw(int throwStrength, Vector2 globalPos, float globalRot) {
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(Global.BIT_MASK_CHAR, true);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
        GetParent().RemoveChild(this);
        LevelNode.AddChild(this);
        GlobalPosition = globalPos;
        GlobalRotation = globalRot;
        Vector2 throwVector = new Vector2(throwStrength, 0);
        ApplyCentralImpulse(throwVector.Rotated(globalRot));
        IsTakable = false;
    }


    [Signal] public delegate void PickedUp();
    private const int KNOCKBACK = 250;


    private void OnWeaponBodyEntered(Godot.Object body) {
        if(body is Player && IsTakable == true) {
            SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            CallDeferred("PickUpDeferred");
        }
        else if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            if(IsTakable == false) {
                enemy.Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), Damage);
            }
        }
        int i = 1;
        if(GD.RandRange(0, 1.0) <= 0.5) {
            i *= -1;
        }
        AngularVelocity = i * 50;
        GD.Print("Weap damage: " + Damage);
    }


    private void PickUpDeferred() {
        GetParent().RemoveChild(this);
        EmitSignal("PickedUp");
    }


}
