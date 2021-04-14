using Godot;
using System;
using System.Collections.Generic;


public class Weapon : RigidBody2D
{
    public Player PlayerNode {get; set;}
    public Level LevelNode {get; set;}
    public int Damage {get; set;}
    public Vector2 Velocity {get; set;}
    public Node2D ItemSlot1Node {get; private set;}
    public WeaponItem Item1 {get; set;}
    public Node2D ItemSlot2Node {get; private set;}
    public WeaponItem Item2 {get; set;}
    public enum States {
        HELD, ACTIVE, INACTIVE
    }
    public States CurrentState {get; private set;}


    public override void _Ready()
    {
        Damage = 1;
    }


    public void RefreshItems() {
        ItemSlot1Node = (Node2D)GetNode("ItemSlot1");
        if(ItemSlot1Node.GetChildCount() != 0) {
            Item1 = (WeaponItem)ItemSlot1Node.GetChild(0);
        }
        ItemSlot2Node = (Node2D)GetNode("ItemSlot2");
        if(ItemSlot2Node.GetChildCount() != 0) {
            Item2 = (WeaponItem)ItemSlot2Node.GetChild(0);
        }
        if(IsInstanceValid(Item1) == true) {
            Item1.WeaponNode = this;
        }
        if(IsInstanceValid(Item2) == true) {
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


    private const int WEAP_MIN_LIN_VEL_LEN = 40000;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(CurrentState == States.ACTIVE && LinearVelocity.LengthSquared() <= WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_CHAR, false);
            if(PlayerNode.WeaponNode == this) {
                SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            }
            CurrentState = States.INACTIVE;
        }
    }


    public void Throw(int throwStrength, Vector2 globalPos, float globalRot) {
        CurrentState = States.ACTIVE;
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(Global.BIT_MASK_CHAR, true);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
        if(IsInstanceValid(GetParent()) == true) {
            GetParent().RemoveChild(this);
        }
        LevelNode.AddChild(this);
        GlobalPosition = globalPos;
        GlobalRotation = globalRot;
        Vector2 throwVector = new Vector2(throwStrength, 0);
        ApplyCentralImpulse(throwVector.Rotated(globalRot));
    }


    [Signal] public delegate void PickedUp();
    private const int KNOCKBACK = 250;


    private void OnWeaponBodyEntered(Godot.Object body) {
        if(body is Player && CurrentState == States.INACTIVE) {
            PickUp();   
        }
        else if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            if(CurrentState == States.ACTIVE) {
                enemy.Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), Damage);
            }
        }
        int i = 1;
        if(GD.RandRange(0, 1.0) <= 0.5) {
            i *= -1;
        }
        AngularVelocity = i * 50;
    }


    public void PickUp() {
        CurrentState = States.HELD;
        SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        CallDeferred("PickUpDeferred");
    }


    private void PickUpDeferred() {
        if(IsInstanceValid(GetParent()) == true) {
            GetParent().RemoveChild(this);
        }
        Position = Vector2.Zero;
        Mode = RigidBody2D.ModeEnum.Static;
        EmitSignal("PickedUp");
    }


}
