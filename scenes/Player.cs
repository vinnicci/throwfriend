using Godot;
using System;

public class Player : Entity
{
    [Export] public int throwStrength = 2000;

    private Node2D center;
    private Position2D weapPos;
    private const int EXTRA_SPEED_WITHOUT_WEAPON = 500;

    public Weapon Weapon {get; set;}
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set {
            levelNode = value;
            Weapon.LevelNode = LevelNode;
        }
    }
    public PlayerItem ItemSlot1 {get; set;}
    public PlayerItem ItemSlot2 {get; set;}


    public override void _Ready()
    {
        base._Ready();
        center = (Node2D)GetNode("Center");
        center.LookAt(GetGlobalMousePosition());
        Weapon = (Weapon)center.GetNode("WeapPos/Weapon");
        Weapon.Connect("PickedUp", this, "PickUpWeapon");
        weapPos = (Position2D)center.GetNode("WeapPos");
        center.LookAt(GetGlobalMousePosition());
        
        // PackedScene itemPack = (PackedScene)ResourceLoader.Load("res://scenes/player items/AutoRetrieve.tscn");
        // ItemSlot1 = (PlayerItem)itemPack.Instance();
        
        if(ItemSlot1 != null) {
            ItemSlot1.PlayerNode = this;
        }
        if(ItemSlot2 != null) {
            ItemSlot2.PlayerNode = this;
        }
    }


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
            Weapon.Throw(throwStrength, new Vector2(GlobalPosition), center.GlobalRotation);
            Speed += EXTRA_SPEED_WITHOUT_WEAPON;
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
        weapPos.AddChild(Weapon);
        Weapon.Mode = RigidBody2D.ModeEnum.Static;
        Weapon.Position = Vector2.Zero;
        Weapon.GlobalRotation = center.GlobalRotation;
        Speed -= EXTRA_SPEED_WITHOUT_WEAPON;
    }




}
