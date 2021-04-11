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


    public override void _PhysicsProcess(float delta)
    {
        stateLabel.Text = IsTakable.ToString();
        if(IsTakable == false && LinearVelocity.Length() <= 250) {
            SetCollisionMaskBit(0, false);
            SetCollisionMaskBit(2, true);
            IsTakable = true;
        }
    }


    public void Throw(int throwStrength, Vector2 pos, float rotation) {
        SetCollisionMaskBit(0, true);
        SetCollisionMaskBit(1, true);
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
            SetCollisionMaskBit(1, false);
            SetCollisionMaskBit(2, false);
            GetParent().RemoveChild(this);
            Sleeping = true;
            LinearVelocity = Vector2.Zero;
            AngularVelocity = 0;
            CallDeferred("PickUpDeferred");
        }
    }


    private void PickUpDeferred() {
        EmitSignal("PickedUp");
    }




}
