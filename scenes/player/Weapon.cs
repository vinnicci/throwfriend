using Godot;
using System;


public class Weapon : RigidBody2D, ITeleportable, ISpawnable
{
    [Export] Texture texture;
    [Export] Texture activeTexture;

    Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            RefreshItems();
            ActivateItem(1);
            ActivateItem(2);
        }
    }
    public int Damage {get; set;}
    public Vector2 Velocity {get; set;}
    public Node2D ItemSlot1Node {get; private set;}
    public WeaponItem Item1 {get; set;}
    public Node2D ItemSlot2Node {get; private set;}
    public WeaponItem Item2 {get; set;}
    public enum States {
        HELD, ACTIVE, INACTIVE
    }
    public States CurrentState {get; private set;}
    public bool IsClone {get; set;}
    public AnimationPlayer TeleportAnim {get; set;}

    Sprite sprite;


    public override void _Notification(int what)
    {
        base._Notification(what);
        if(what == NotificationInstanced) {
            Damage = 1;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("Sprite");
        TeleportAnim = (AnimationPlayer)GetNode("TeleAnim");
        sprite.Texture = texture;
    }


    public void RefreshItems() {
        ItemSlot1Node = (Node2D)GetNode("ItemSlot1");
        if(ItemSlot1Node.GetChildCount() != 0) {
            Item1 = (WeaponItem)ItemSlot1Node.GetChild(0);
        }
        ItemSlot2Node = (Node2D)GetNode("ItemSlot2");
        if(ItemSlot2Node.GetChildCount() != 0) {
            Item2 = (WeaponItem)ItemSlot2Node.GetChild(0);
        }
    }


    public void ActivateItem(int slotNum) {
        if(slotNum == 1 && IsInstanceValid(Item1) == true) {
            Item1.WeaponNode = this;
        }
        else if(slotNum == 2 && IsInstanceValid(Item2) == true) {
            Item2.WeaponNode = this;
        }
        RefreshItems();
    }


    Vector2 teleportPos;


    public void Teleport(Level level, Vector2 global_pos) {
        //sprite fade out effect
        Sprite teleSprite = (Sprite)GetNode("Sprite").Duplicate();
        level.AddChild(teleSprite);
        Tween tween = new Tween();
        teleSprite.AddChild(tween);
        teleSprite.GlobalPosition = GlobalPosition;
        teleSprite.GlobalRotation = GlobalRotation;
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(teleSprite);
        tween.Connect("tween_all_completed", this, nameof(FreeSprite), arr);
        tween.InterpolateProperty(teleSprite, "modulate",
        new Color(1,1,1,1), new Color(1,1,1,0), 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
        //teleport
        teleportPos = global_pos;
        TeleportAnim.Play("teleported");
    }


    public void FreeSprite(Godot.Object teleSprite) {
        ((Sprite)teleSprite).QueueFree();
    }


    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        //teleport
        if(teleportPos != Vector2.Zero) {
            Transform2D trans = new Transform2D(Rotation, teleportPos);
            state.Transform = trans;
            teleportPos = Vector2.Zero;
        }
        //velocity
        AppliedForce = Vector2.Zero;
        state.AddCentralForce(Velocity);
        Velocity = Vector2.Zero;
    }


    const int WEAP_MIN_LIN_VEL_LEN = 15625;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        int lv = (int)LinearVelocity.LengthSquared();
        if(ContinuousCd == RigidBody2D.CCDMode.Disabled && lv > Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.CastRay;
        }
        else if(ContinuousCd == RigidBody2D.CCDMode.CastRay && lv <= Global.CCD_MAX) {
            ContinuousCd = RigidBody2D.CCDMode.Disabled;
        }
        if(CurrentState == States.ACTIVE && lv <= WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_ENEMY, false);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            CurrentState = States.INACTIVE;
        }
        else if(CurrentState == States.INACTIVE && lv > WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_ENEMY, true);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            CurrentState = States.ACTIVE;
        }
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(GetCollisionMaskBit(Global.BIT_MASK_ENEMY) == true && sprite.Texture == texture) {
            sprite.Texture = activeTexture;
        }
        else if(GetCollisionMaskBit(Global.BIT_MASK_ENEMY) == false && sprite.Texture == activeTexture) {
            sprite.Texture = texture;
        }
    }


    public void Throw(int throwStrength, Vector2 globalPos, Vector2 destination, float globalRot) {
        CurrentState = States.ACTIVE;
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(Global.BIT_MASK_ENEMY, true);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        SetCollisionMaskBit(Global.BIT_MASK_LVL, true);
        if(IsInstanceValid(GetParent()) == true) {
            GetParent().RemoveChild(this);
        }
        Spawn(PlayerNode.LevelNode, globalPos, Vector2.Zero, globalRot);
        Vector2 throwVector = new Vector2(throwStrength, 0);
        ApplyCentralImpulse(throwVector.Rotated(GlobalRotation));
    }


    public void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0) {
        lvl.AddChild(this);
        GlobalPosition = globalPos;
        GlobalRotation = globalRot;
    }


    [Signal] public delegate void PickedUp();
    const int KNOCKBACK = 125;
    const int LARGE_KNOCKBACK = 250;
    const int BOUNCE_ROTATION = 30;


    void OnWeaponBodyEntered(Godot.Object body) {
        if(body is Player && CurrentState == States.INACTIVE) {
            if(PlayerNode.WeaponNode == this) {
                OnPickedUp();
            }
            else {
                SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            }
        }
        else if(body is IHealthModifiable) {
            IHealthModifiable hitBody = (IHealthModifiable)body;
            int dmg = Damage * PlayerNode.SnarkDmgMult;
            int knockback = KNOCKBACK;
            if(Filename == Global.WEAP_LARGE_FILE) {
                knockback = LARGE_KNOCKBACK;
            }
            if(CurrentState == States.ACTIVE) {
                hitBody.Hit(new Vector2(knockback, 0).Rotated(GlobalRotation), dmg);
            }
            else if(CurrentState == States.INACTIVE &&
            (PlayerNode.Item1 is AutoRetrieve || PlayerNode.Item2 is AutoRetrieve)) {
                hitBody.Hit(new Vector2(knockback, 0).Rotated(GlobalRotation), dmg);
            }
        }
        int i = 1;
        if(GD.RandRange(0, 1.0) <= 0.5) {
            i *= -1;
        }
        AngularVelocity = i * BOUNCE_ROTATION;
    }


    public void OnPickedUp() {
        CurrentState = States.HELD;
        SetCollisionMaskBit(Global.BIT_MASK_LVL, false);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        CallDeferred(nameof(PickUpDeferred));
    }


    void PickUpDeferred() {
        if(IsInstanceValid(GetParent()) == true) {
            GetParent().RemoveChild(this);
        }
        Position = Vector2.Zero;
        Mode = RigidBody2D.ModeEnum.Static;
        EmitSignal(nameof(PickedUp));
    }


}
