using Godot;
using System;

public class Weapon : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Main LevelNode {get; set;}
    public bool IsTakable {get; private set;}


    private Label stateLabel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        stateLabel = (Label)GetNode("Label");
        IsTakable = true;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    // }


    const int WEAP_MIN_LIN_VEL_LEN = 300;
    const short BIT_MASK_CHAR = 0;
    const short BIT_MASK_LVL = 1;
    const short BIT_MASK_PLAYER = 2;


    public override void _PhysicsProcess(float delta)
    {
        stateLabel.Text = IsTakable.ToString();
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


    private void OnWeaponBodyEntered(Godot.Object body) {
        if(body is Player && IsTakable == true) {
            SetCollisionMaskBit(BIT_MASK_LVL, false);
            SetCollisionMaskBit(BIT_MASK_PLAYER, false);
            CallDeferred("PickUpDeferred");
        }
    }


    private void PickUpDeferred() {
        GetParent().RemoveChild(this);
        EmitSignal("PickedUp");
    }




}
