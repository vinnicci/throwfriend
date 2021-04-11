using Godot;
using System;

public class Weapon : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Main LevelNode {get; set;}
    public bool IsReleased {get; private set;}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("weap ready");
    }


    public void OnThrow(int throwStrength, Vector2 pos, float rotation) {
        if(IsReleased == true) {
            return;
        }
        IsReleased = true;
        SetCollisionMaskBit(0, true);
        SetCollisionMaskBit(1, true);
        GetParent().RemoveChild(this);
        GlobalPosition = pos;
        LevelNode.AddChild(this);
        Vector2 throwVector = new Vector2(throwStrength, 0);
        ApplyCentralImpulse(throwVector.Rotated(rotation));
    }


 // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    // }




}
