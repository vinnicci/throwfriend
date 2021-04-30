using Godot;
using System;

public class Player : Entity
{
    [Export] protected int throwStrength = 2000;
    public int ThrowStrength {get; set;}
    public Node2D Center {get; set;}
    public Weapon WeaponNode {get; set;}
    private Level levelNode;
    public Level LevelNode {
        get {
            return levelNode;
        }
        set {
            levelNode = value;
            RefreshItems();
            ActivateItem(1);
            ActivateItem(2);
            WeaponNode.PlayerNode = this;
            WeaponNode.LevelNode = LevelNode;
            inGameUI.PlayerNode = this;
            inGameUI.WeaponNode = WeaponNode;
            hotkeyHUD.PlayerNode = this;
            hotkeyHUD.WeaponNode = WeaponNode;
            weapSprite = (Sprite)WeaponNode.GetNode("Sprite");
        }
    }
    private Node2D itemSlot1Node;
    public PlayerItem Item1 {get; set;}
    private Node2D itemSlot2Node;
    public PlayerItem Item2 {get; set;}
    
    private Position2D weapPos;
    private PlayerCam camera;
    private Sprite head;
    private Sprite arms;
    private Sprite weapSprite;
    private InGame inGameUI;
    private AnimationPlayer inGameUIAnim;
    private Loadout loadout;
    private HotkeyHUD hotkeyHUD;
    
    private const int EXTRA_SPEED_WITHOUT_WEAPON = 500;


    public override void _Ready()
    {
        base._Ready();
        ThrowStrength = throwStrength;
        Center = (Node2D)GetNode("Center");
        Center.LookAt(GetGlobalMousePosition());
        camera = (PlayerCam)GetNode("PlayerCam");
        camera.ParentNode = this;
        WeaponNode = (Weapon)Center.GetNode("WeapPos/Weapon");
        WeaponNode.Connect("PickedUp", this, "PickUpWeapon");
        weapPos = (Position2D)Center.GetNode("WeapPos");
        Center.LookAt(GetGlobalMousePosition());
        head = (Sprite)sprite.GetNode("Head");
        arms = (Sprite)GetNode("Center/Arms");
        inGameUI = (InGame)GetNode("CanvasLayer/InGame");
        inGameUIAnim = (AnimationPlayer)inGameUI.GetNode("Anim");
        loadout = (Loadout)inGameUI.GetNode("Loadout");
        hotkeyHUD = (HotkeyHUD)GetNode("CanvasLayer/HotkeyHUD");
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
    }


    public void ActivateItem(int slotNum) {
        if(slotNum == 1 && IsInstanceValid(Item1) == true) {
            Item1.PlayerNode = this;
        }
        else if(slotNum == 2 && IsInstanceValid(Item2) == true) {
            Item2.PlayerNode = this;
        }
    }


    private Vector2 WEAP_POS_VEC = new Vector2(-35, -22);
    private Vector2 WEAP_POS_VEC_FLIP = new Vector2(-35, 22); 


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(IsDead == true) {
            return;
        }
        Vector2 look = new Vector2(GetGlobalMousePosition());
        Center.LookAt(look);
        float dotProd = new Vector2(1,0).Dot(new Vector2(1,0).Rotated(Center.Rotation));
        if(dotProd <= 0 && head.FlipH == false) {
            head.FlipH = true;
            arms.FlipH = true;
            legs.FlipH = true;
            weapPos.Position = WEAP_POS_VEC_FLIP;
            weapSprite.FlipV = true;
        }
        else if(dotProd > 0 && head.FlipH == true) {
            head.FlipH = false;
            arms.FlipH = false;
            legs.FlipH = false;
            weapPos.Position = WEAP_POS_VEC;
            weapSprite.FlipV = false;
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        GetInput();
    }


    public void GetInput() {
        if(IsDead == true) {
            return;
        }
        bool hasWeap = Center.HasNode("WeapPos/Weapon");
        //in game ui
        if(Input.IsActionJustPressed("in_game_ui") && hasWeap == true) {
            if(inGameUIAnim.IsPlaying() == false) {
                if(inGameUI.Visible == false) {
                    inGameUIAnim.Play("enter");
                }
                else if(inGameUI.Visible == true) {
                    inGameUIAnim.Play("exit");
                }
            }
        }
        //velocity
        Vector2 velocity = Vector2.Zero;
        if(Input.IsActionPressed("up")) {
            velocity.y -= 1;
        }
        else if(Input.IsActionPressed("down")) {
            velocity.y += 1;
        }
        if(Input.IsActionPressed("left")) {
            velocity.x -= 1;
        }
        else if(Input.IsActionPressed("right")) {
            velocity.x += 1;
        }
        Velocity = velocity;
        if(inGameUI.Visible == true) {
            return;
        }
        //throw
        if(Input.IsActionJustReleased("throw_weap") && hasWeap == true) {
            WeaponNode.Throw(ThrowStrength, GlobalPosition, Center.GlobalRotation);
            if(arms.FlipH == false) {
                anim.Play("throw");
            }
            else {
                anim.PlayBackwards("throw");
            }
            Speed += EXTRA_SPEED_WITHOUT_WEAPON;
        }
        if(IsInstanceValid(Item1) == true && Input.IsActionJustPressed("hotkey_1")) {
            Item1.ApplyEffect();
        }
        if(IsInstanceValid(Item2) == true && Input.IsActionJustPressed("hotkey_2")) {
            Item2.ApplyEffect();
        }
        if(IsInstanceValid(WeaponNode.Item1) == true && Input.IsActionJustPressed("hotkey_3")) {
            WeaponNode.Item1.ApplyEffect();
        }
        if(IsInstanceValid(WeaponNode.Item2) == true && Input.IsActionJustPressed("hotkey_4")) {
            WeaponNode.Item2.ApplyEffect();
        }
    }


    private void PickUpWeapon() {
        weapPos.AddChild(WeaponNode);
        float rot = Center.GlobalRotation;
        WeaponNode.GlobalRotation = rot;
        Speed -= EXTRA_SPEED_WITHOUT_WEAPON;
    }


    public override void OnDeathTimerTimeout() {
        camera.GetParent().RemoveChild(camera);
        LevelNode.AddChild(camera);
        camera.ParentNode = LevelNode;
        camera.GlobalPosition = GlobalPosition;
        base.OnDeathTimerTimeout();
    }


    public override void Hit(Vector2 linearV, int damage)
    {
        if(hitCooldown.IsStopped() == false) {
            return;
        }
        base.Hit(linearV, damage);
        if(Health > 0) {
            hitCooldown.Start();
        }
    }


}
