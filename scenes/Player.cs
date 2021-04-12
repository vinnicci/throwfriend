using Godot;
using System;

public class Player : Entity
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public int throwStrength = 2000;

    private Node2D center;
    private Position2D weapPos;

    public Weapon weapon;
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set {
            levelNode = value;
            weapon.LevelNode = LevelNode;
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        center = (Node2D)GetNode("Center");
        center.LookAt(GetGlobalMousePosition());
        weapon = (Weapon)center.GetNode("WeapPos/Weapon");
        weapon.Connect("PickedUp", this, "PickUpWeapon");
        weapPos = (Position2D)center.GetNode("WeapPos");
    }


    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(center.HasNode("WeapPos/Weapon")) {
            center.LookAt(GetGlobalMousePosition());
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        GetInput();
    }


    public void GetInput() {
        if(Input.IsActionJustReleased("throw_weap") && center.HasNode("WeapPos/Weapon") == true) {
            weapon.Throw(throwStrength, new Vector2(GlobalPosition), center.GlobalRotation);
        }
        Vector2 velocity = Vector2.Zero;
        if(Input.IsActionPressed("up")) {
            velocity.y -= 1;
        }
        if(Input.IsActionPressed("down")) {
            velocity.y += 1;
        }
        if(Input.IsActionPressed("left")) {
            velocity.x -= 1;
        }
        if(Input.IsActionPressed("right")) {
            velocity.x += 1;
        }
        Velocity = velocity;
    }


    private void PickUpWeapon() {
        weapPos.AddChild(weapon);
        weapon.Mode = RigidBody2D.ModeEnum.Static;
        weapon.Position = Vector2.Zero;
        weapon.GlobalRotation = center.GlobalRotation;
    }




}
