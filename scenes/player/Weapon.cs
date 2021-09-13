using Godot;
using System;


public class Weapon : RigidBody2D, ITeleportable, ISpawnable
{
    public WeaponItem Item1 {get; set;}
    public WeaponItem Item2 {get; set;}
    public int Damage {get; set;}

    Player playerNode;
    public Player PlayerNode {
        get {
            return playerNode;
        }
        set {
            playerNode = value;
            ActivateItem(1);
            ActivateItem(2);
        }
    }
    public Vector2 Velocity {get; set;}
    public Node2D ItemSlot1Node {get; private set;}
    public Node2D ItemSlot2Node {get; private set;}
    public enum States {
        HELD, ACTIVE, INACTIVE
    }
    public States CurrentState {get; private set;}
    public bool IsClone {get; set;}
    public AnimationPlayer TeleportAnim {get; set;}
    public AnimationPlayer Anim {get; private set;}

    Sprite inactiveSprite;
    Sprite activeSprite;
    //used for stuck in a wall bug
    Godot.Collections.Array stuckCondArr = new Godot.Collections.Array();
    

    public override void _Notification(int what)
    {
        base._Notification(what);
        if(what == NotificationInstanced) {
            Damage = 1;
            ItemSlot1Node = (Node2D)GetNode("ItemSlot1");
            ItemSlot2Node = (Node2D)GetNode("ItemSlot2");
        }
    }


    public override void _Ready()
    {
        base._Ready();
        inactiveSprite = (Sprite)GetNode("Sprite");
        activeSprite = (Sprite)GetNode("Sprite2");
        Anim = (AnimationPlayer)GetNode("Anim");        
        TeleportAnim = (AnimationPlayer)GetNode("TeleAnim");
        stuckCondArr.Add(false);
        stuckCondArr.Add(0);
    }


    public void RefreshItems() {
        if(ItemSlot1Node.GetChildCount() != 0) {
            Item1 = (WeaponItem)ItemSlot1Node.GetChild(0);
        }
        if(ItemSlot2Node.GetChildCount() != 0) {
            Item2 = (WeaponItem)ItemSlot2Node.GetChild(0);
        }
    }


    public void ActivateItem(int slotNum) {
        RefreshItems();
        if(slotNum == 1 && IsInstanceValid(Item1)) {
            Item1.WeaponNode = this;
        }
        else if(slotNum == 2 && IsInstanceValid(Item2)) {
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
        if(tween.IsConnected("tween_all_completed", PlayerNode.LevelNode, nameof(Level.QueueFreeObject)) == false) {
            tween.Connect("tween_all_completed", PlayerNode.LevelNode, nameof(Level.QueueFreeObject), arr);
        }
        tween.InterpolateProperty(teleSprite, "modulate",
        new Color(1,1,1,1), new Color(1,1,1,0), 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
        //teleport
        teleportPos = global_pos;
        TeleportAnim.Play("teleported");
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


    const int WEAP_MIN_LIN_VEL_LEN = 22500;


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
            if(IsClone == false) {
                SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
            }
            CurrentState = States.INACTIVE;
            if(Anim.IsPlaying() == false) {
                inactiveSprite.Visible = true;
                activeSprite.Visible = false;
            }
        }
        else if(CurrentState == States.INACTIVE && lv > WEAP_MIN_LIN_VEL_LEN) {
            SetCollisionMaskBit(Global.BIT_MASK_ENEMY, true);
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
            CurrentState = States.ACTIVE;
            if(Anim.IsPlaying() == false) {
                inactiveSprite.Visible = false;
                activeSprite.Visible = true;
            }
        }
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        //unstuck from wall
        if((bool)stuckCondArr[0] == true) {
            if((float)stuckCondArr[1] > 0) {
                stuckCondArr[1] = (float)stuckCondArr[1] - delta;
            }
            else if((float)stuckCondArr[1] <= 0) {
                stuckCondArr[0] = false;
                stuckCondArr[1] = 0;
                Teleport(PlayerNode.LevelNode, PlayerNode.GlobalPosition);
            }
        }
    }


    public void Throw(int throwStrength, Vector2 globalPos, Vector2 destination, float globalRot) {
        CurrentState = States.ACTIVE;
        if(Anim.IsPlaying() == false) {
            inactiveSprite.Visible = false;
            activeSprite.Visible = true;
        }
        Mode = RigidBody2D.ModeEnum.Rigid;
        SetCollisionMaskBit(Global.BIT_MASK_ENEMY, true);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        SetCollisionMaskBit(Global.BIT_MASK_WALL, true);
        SetCollisionMaskBit(Global.BIT_MASK_SEETHROUGH_WALL, true);
        if(IsInstanceValid(GetParent())) {
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
    const int BOUNCE_ROTATION = 30;


    void OnWeaponBodyEntered(Godot.Object body) {
        if(CurrentState == States.HELD) {
            return;
        }
        if(body is Player && CurrentState == States.INACTIVE && PlayerNode.WeaponNode == this) {
            OnPickedUp();
        }
        else if(body is IHealthModifiable) {
            IHealthModifiable hitBody = (IHealthModifiable)body;
            int dmg = Damage * PlayerNode.SnarkDmgMult;
            //damage x2 if player has superthrow ability
            if((IsInstanceValid(PlayerNode.Item1) && PlayerNode.Item1 is SuperThrow) ||
            (IsInstanceValid(PlayerNode.Item2) && PlayerNode.Item2 is SuperThrow)) {
                dmg *= 2;
            }
            if(CurrentState == States.ACTIVE) {
                hitBody.Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), dmg);
            }
            else if(CurrentState == States.INACTIVE &&
            (PlayerNode.Item1 is AutoRetrieve || PlayerNode.Item2 is AutoRetrieve)) {
                hitBody.Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), dmg);
            }
        }
        if(body is LevelTiles) {
            stuckCondArr[0] = true;
            stuckCondArr[1] = 1f;
        }
        int i = 1;
        if(GD.RandRange(0, 1.0) <= 0.5) {
            i *= -1;
        }
        AngularVelocity = i * BOUNCE_ROTATION;
    }


    void OnWeaponBodyExited(Godot.Object body) {
        if(body is LevelTiles) {
            stuckCondArr[0] = false;
            stuckCondArr[1] = 0;
        }
    }


    public void OnPickedUp() {
        CurrentState = States.HELD;
        SetCollisionMaskBit(Global.BIT_MASK_WALL, false);
        SetCollisionMaskBit(Global.BIT_MASK_SEETHROUGH_WALL, false);
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        CallDeferred(nameof(PickUpDeferred));
    }


    void PickUpDeferred() {
        if(IsInstanceValid(GetParent())) {
            GetParent().RemoveChild(this);
        }
        Position = Vector2.Zero;
        Mode = RigidBody2D.ModeEnum.Static;
        EmitSignal(nameof(PickedUp));
    }


    public void BeamAttack() {
        Anim.Play("beam");
    }


    void OnSnarkBeamBodyEntered(Godot.Object body) {
        if(body is Enemy) {
            ((Entity)body).Hit(new Vector2(KNOCKBACK, 0).Rotated(GlobalRotation), Damage * PlayerNode.SnarkDmgMult);
            ((Enemy)body).Engage();
        }
    }


}
