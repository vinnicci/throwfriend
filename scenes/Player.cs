using Godot;
using System;

public class Player : Entity
{
    [Export] protected int throwStrength = 2000;
    public int ThrowStrength {get; set;}
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
    private Node2D itemSlot1Node;
    public PlayerItem Item1 {get; set;}
    private Node2D itemSlot2Node;
    public PlayerItem Item2 {get; set;}


    public override void _Ready()
    {
        base._Ready();
        ThrowStrength = throwStrength;
        center = (Node2D)GetNode("Center");
        center.LookAt(GetGlobalMousePosition());
        Weapon = (Weapon)center.GetNode("WeapPos/Weapon");
        Weapon.Connect("PickedUp", this, "PickUpWeapon");
        weapPos = (Position2D)center.GetNode("WeapPos");
        center.LookAt(GetGlobalMousePosition());
        RefreshItems();
    }


    public void RefreshItems() {
        itemSlot1Node = (Node2D)GetNode("ItemSlot1");
        if(itemSlot1Node.GetChildCount() != 0) {
            Item1 = (PlayerItem)itemSlot1Node.GetChild(0);
            Item1.PlayerNode = this;
        }
        itemSlot2Node = (Node2D)GetNode("ItemSlot2");
        if(itemSlot2Node.GetChildCount() != 0) {
            Item2 = (PlayerItem)itemSlot2Node.GetChild(0);
            Item2.PlayerNode = this;
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
            Weapon.Throw(ThrowStrength, new Vector2(GlobalPosition), center.GlobalRotation);
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
