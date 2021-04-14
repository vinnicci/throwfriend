using Godot;
using System;

public class Player : Entity
{
    [Export] protected int throwStrength = 2000;
    public int ThrowStrength {get; set;}
    public Node2D Center {get; set;}
    private Position2D weapPos;
    private const int EXTRA_SPEED_WITHOUT_WEAPON = 500;
    public Weapon WeaponNode {get; set;}
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set {
            levelNode = value;
            RefreshItems();
            WeaponNode.PlayerNode = this;
            WeaponNode.LevelNode = LevelNode;
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
        Center = (Node2D)GetNode("Center");
        Center.LookAt(GetGlobalMousePosition());
        WeaponNode = (Weapon)Center.GetNode("WeapPos/Weapon");
        WeaponNode.Connect("PickedUp", this, "PickUpWeapon");
        weapPos = (Position2D)Center.GetNode("WeapPos");
        Center.LookAt(GetGlobalMousePosition());
    }


    public void RefreshItems() {
        itemSlot1Node = (Node2D)GetNode("ItemSlot1");
        if(itemSlot1Node.GetChildCount() != 0) {
            Item1 = (PlayerItem)itemSlot1Node.GetChild(0);
        }
        itemSlot2Node = (Node2D)GetNode("ItemSlot2");
        if(itemSlot2Node.GetChildCount() != 0) {
            Item2 = (PlayerItem)itemSlot2Node.GetChild(0);
        }
        if(IsInstanceValid(Item1) == true) {
            Item1.PlayerNode = this;
        }
        if(IsInstanceValid(Item2) == true) {
            Item2.PlayerNode = this;
        }
    }


    public override void _Process(float delta)
    {
        if(Center.HasNode("WeapPos/Weapon")) {
            Center.LookAt(GetGlobalMousePosition());
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        GetInput();
    }


    public void GetInput() {
        if(Input.IsActionJustReleased("throw_weap") && Center.HasNode("WeapPos/Weapon") == true) {
            WeaponNode.Throw(ThrowStrength, GlobalPosition, Center.GlobalRotation);
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
        weapPos.AddChild(WeaponNode);
        float rot = Center.GlobalRotation;
        WeaponNode.GlobalRotation = rot;
        Speed -= EXTRA_SPEED_WITHOUT_WEAPON;
    }


}
