using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public int speed = 100;
    [Export] public int throwStrength = 2000;

    private Node2D center;

    public Vector2 Velocity {get; set;}
    public Weapon weapon;
    private Main levelNode;
    public Main LevelNode {
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
        weapon = (Weapon)center.GetNode("Weapon");
        weapon.Position = new Vector2(-45,0);
        center.LookAt(GetGlobalMousePosition());
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    // }


    public override void _PhysicsProcess(float delta)
    {
        center.LookAt(GetGlobalMousePosition());
        GetInput();
        MoveAndSlide(Velocity);
    }


    public void GetInput() {
        Vector2 velocity = new Vector2();
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
        Velocity = velocity.Clamped(1) * speed;
        if(Input.IsActionJustReleased("throw_weap")) {
            weapon.OnThrow(throwStrength, GlobalPosition, center.GlobalRotation);
        }
    }






}
