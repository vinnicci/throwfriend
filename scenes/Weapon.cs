using Godot;
using System;

public class Weapon : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Level LevelNode {get; set;}
    public bool IsTakable {get; private set;}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        IsTakable = true;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    // }


    private const int WEAP_MIN_LIN_VEL_LEN = 350;
    private const short BIT_MASK_CHAR = 0;
    private const short BIT_MASK_LVL = 1;
    private const short BIT_MASK_PLAYER = 2;


    public override void _PhysicsProcess(float delta)
    {
        if(IsTakable == false && LinearVelocity.Length() <= WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(BIT_MASK_CHAR, false);
            SetCollisionMaskBit(BIT_MASK_PLAYER, true);
            IsTakable = true;
        }
    }


    public void Throw(int throwStrength, Vector2 pos, float rotation) {
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(BIT_MASK_CHAR, true);
        SetCollisionMaskBit(BIT_MASK_LVL, true);
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


    private void OnWeaponBodyEntered(Godot.Object body) {
        AngularVelocity = 50;
        if(body is Player && IsTakable == true) {
            SetCollisionMaskBit(BIT_MASK_LVL, false);
            SetCollisionMaskBit(BIT_MASK_PLAYER, false);
            CallDeferred("PickUpDeferred");
        }
        else if(body is Enemy && IsTakable == false && hitList.Contains(body) == false) {
            Enemy enemy = (Enemy)body;
            enemy.Hit(LinearVelocity * (float)0.5, 1);
            hitList.Add(enemy);
        }
    }


    private void PickUpDeferred() {
        GetParent().RemoveChild(this);
        EmitSignal("PickedUp");
    }




}
