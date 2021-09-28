using Godot;
using System;

public class Player : Entity
{
    [Export] protected int throwStrength = 100;
    public int ThrowStrength {get; set;}
    public PlayerItem Item1 {get; set;}
    public PlayerItem Item2 {get; set;}
    public int AvailableUpgrade {get; set;}
    public int SnarkDmgMult {get; set;}
    public Node2D Center {get; set;}
    public Weapon WeaponNode {get; set;}
    public override Level LevelNode {
        get {
            return levelNode;
        }
        set {
            base.LevelNode = value;
            WeaponNode.PlayerNode = this;
            inGameUI.PlayerNode = this;
            ActivateItem(1);
            ActivateItem(2);
        }
    }
    public Node2D ItemSlot1Node {get; private set;}
    public Node2D ItemSlot2Node {get; private set;}
    public enum StatusEffect {
        CONFUSE, SLOW
    }
    public bool[] CurrentStatusEffect {get; set;}
    public const int EXTRA_SPEED_WITHOUT_WEAPON = 250;
    
    Position2D weapPos;
    PlayerCam camera;
    Sprite head;
    Sprite arms;
    InGame inGameUI;
    AnimationPlayer inGameUIAnim;
    Loadout loadout;
    StatsDesc statsDesc;
    Settings settings;
    HotkeyHUD hotkeyHUD;
    WarningText uiWarning;
    AnimationPlayer throwAnim;
    Node2D snarkPointer;
    Sprite confusedIndicator;
    Sprite slowedIndicator;


    public override void _Notification(int what)
    {
        base._Notification(what);
        if(what == NotificationInstanced) {
            ThrowStrength = throwStrength;
            AvailableUpgrade = 0;
            SnarkDmgMult = 1;
            ItemSlot1Node = (Node2D)GetNode("ItemSlot1");
            ItemSlot2Node = (Node2D)GetNode("ItemSlot2");
        }
    }


    public override void _Ready()
    {
        base._Ready();
        Center = (Node2D)GetNode("Center");
        camera = (PlayerCam)GetNode("PlayerCam");
        camera.ParentNode = this;
        weapPos = (Position2D)Center.GetNode("WeapPos");
        WeaponNode = (Weapon)Center.GetNode("WeapPos/Weapon");
        WeaponNode.Connect(nameof(Weapon.PickedUp), this, nameof(PickedUpWeapon));
        head = (Sprite)spriteNode.GetNode("Head");
        arms = (Sprite)GetNode("Center/Arms");
        inGameUI = (InGame)GetNode("CanvasLayer/InGame");
        inGameUIAnim = (AnimationPlayer)inGameUI.GetNode("Anim");
        loadout = (Loadout)inGameUI.GetNode("Loadout");
        statsDesc = (StatsDesc)inGameUI.GetNode("StatsDesc");
        settings = (Settings)inGameUI.GetNode("Settings");
        hotkeyHUD = (HotkeyHUD)GetNode("CanvasLayer/HotkeyHUD");
        loadout.HotkeyHUDNode = hotkeyHUD;
        uiWarning = (WarningText)GetNode("CanvasLayer/UIWarning");
        throwAnim = (AnimationPlayer)GetNode("Anims/ThrowAnim");
        snarkPointer = (Node2D)GetNode("SnarkPointer");
        confusedIndicator = (Sprite)hud.GetNode("Confused");
        slowedIndicator = (Sprite)hud.GetNode("Slowed");
        CurrentStatusEffect = new bool[2];
        Center.LookAt(GetGlobalMousePosition());
        Center.LookAt(GetGlobalMousePosition());
    }


    public void UpdateWeapon(Weapon newWeap) {
        Weapon temp = WeaponNode;
        temp.GetParent().RemoveChild(temp);
        weapPos.AddChild(newWeap);
        WeaponNode = newWeap;
        WeaponNode.PlayerNode = this;
        WeaponNode.Connect(nameof(Weapon.PickedUp), this, nameof(PickedUpWeapon));
        temp.QueueFree();
    }


    public void RefreshItems() {
        if(ItemSlot1Node.GetChildCount() != 0) {
            Item1 = (PlayerItem)ItemSlot1Node.GetChild(0);
        }
        if(ItemSlot2Node.GetChildCount() != 0) {
            Item2 = (PlayerItem)ItemSlot2Node.GetChild(0);
        }
        UpdateStatsDisp();
    }


    public void ActivateItem(int slotNum) {
        RefreshItems();
        if(slotNum == 1 && IsInstanceValid(Item1)) {
            Item1.PlayerNode = this;
        }
        else if(slotNum == 2 && IsInstanceValid(Item2)) {
            Item2.PlayerNode = this;
        }
        RefreshItems();
    }


    Vector2 WEAP_POS_VEC = new Vector2(-18, -10);
    Vector2 WEAP_POS_VEC_FLIP = new Vector2(-18, 10); 


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(Health <= 0) {
            return;
        }
        Center.LookAt(GetGlobalMousePosition());
        float dotProd = new Vector2(1,0).Dot(new Vector2(1,0).Rotated(Center.Rotation));
        if(dotProd <= 0 && arms.Scale.x > 0) {
            Vector2 scale = new Vector2(-1,1);
            spriteNode.Scale *= scale;
            arms.Scale *= scale;
            weapPos.Position = WEAP_POS_VEC_FLIP;
        }
        else if(dotProd > 0 && arms.Scale.x < 0) {
            Vector2 scale = new Vector2(-1,1);
            spriteNode.Scale *= scale;
            arms.Scale *= scale;
            weapPos.Position = WEAP_POS_VEC;
        }
    }


    public override void AdjustSprites()
    {
        if(Velocity == Vector2.Zero) {
            anim.Play("idle");
        }
        else {
            FlipLegs(spriteNode.Scale.x, Velocity.x);
        }
    }


    void FlipLegs(float sprite, float velocity) {
        if((sprite > 0 && velocity > 0) || (sprite < 0 && velocity <= 0)) {
            anim.Play("run");
        }
        else if((sprite < 0 && velocity > 0) || (sprite > 0 && velocity <= 0)) {
            anim.Play("run_back");
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        GetInput();
    }


    public void WarnPlayer(String warning) {
        if(warning == "") {
            return;
        }
        uiWarning.Text = warning;
        uiWarning.ShowWarning();
    }


    [Signal] public delegate void ActivatedWeaponItem();
    const float SLOW_EFFECT = 0.3f;


    public bool IsStopped {get; set;}


    public void GetInput() {
        if(IsStopped || Health <= 0) {
            return;
        }
        bool hasWeap = Center.HasNode("WeapPos/Weapon");
        //in game ui
        if(Input.IsActionJustPressed("in_game_ui") && settings.Visible == false &&
        inGameUIAnim.IsPlaying() == false) {
            if(hasWeap == false) {
                WarnPlayer("YOU MUST CARRY SNARK TO OPEN THE IN-GAME MENU");
            }
            else if(inGameUI.Visible == false) {
                inGameUIAnim.Play("enter");
            }
            else if(inGameUI.Visible) {
                inGameUIAnim.Play("exit");
            }
        }
        //snark pointer
        if(snarkPointer.Visible) {
            snarkPointer.LookAt(WeaponNode.GlobalPosition);
        }
        //velocity
        if(settings.Visible) {
            return;
        }
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
        Velocity = velocity.Normalized();
        //movement status effect
        if(CurrentStatusEffect[(int)StatusEffect.CONFUSE]) {
            Velocity *= -1;
            if(confusedIndicator.Visible == false) {
                confusedIndicator.Visible = true;
            }
        }
        if(CurrentStatusEffect[(int)StatusEffect.SLOW]) {
            Velocity *= SLOW_EFFECT;
            if(slowedIndicator.Visible == false) {
                slowedIndicator.Visible = true;
            }
        }
        if(inGameUI.Visible) {
            return;
        }
        //throw
        if(Input.IsActionJustReleased("throw_weap") && hasWeap) {
            WeaponNode.Throw(ThrowStrength, GlobalPosition, Vector2.Zero, Center.GlobalRotation);
            if(arms.FlipH == false) {
                throwAnim.Play("throw");
            }
            else {
                throwAnim.PlayBackwards("throw");
            }
            snarkPointer.Visible = true;
            Speed += EXTRA_SPEED_WITHOUT_WEAPON;
        }
        if(IsInstanceValid(Item1) && Input.IsActionJustPressed("hotkey_1")) {
            Item1.ApplyEffect();
        }
        if(IsInstanceValid(Item2) && Input.IsActionJustPressed("hotkey_2")) {
            Item2.ApplyEffect();
        }
        if(IsInstanceValid(WeaponNode.Item1) && Input.IsActionJustPressed("hotkey_3")) {
            if(WeaponNode.Item1.Cooldown.IsStopped()) {
                EmitSignal(nameof(ActivatedWeaponItem), 1);
            }
            WeaponNode.Item1.ApplyEffect();
        }
        if(IsInstanceValid(WeaponNode.Item2) && Input.IsActionJustPressed("hotkey_4")) {
            if(WeaponNode.Item2.Cooldown.IsStopped()) {
                EmitSignal(nameof(ActivatedWeaponItem), 2);
            }
            WeaponNode.Item2.ApplyEffect();
        }
    }


    public void ClearStatusEffect(StatusEffect eff, Timer timer) {
        if(eff == StatusEffect.CONFUSE) {
            CurrentStatusEffect[(int)StatusEffect.CONFUSE] = false;
            confusedIndicator.Visible = false;
        }
        else if(eff == StatusEffect.SLOW) {
            CurrentStatusEffect[(int)StatusEffect.SLOW] = false;
            slowedIndicator.Visible = false;
        }
        if(IsInstanceValid(timer)) {
            timer.QueueFree();
        }
    }


    public void PickedUpWeapon() {
        weapPos.AddChild(WeaponNode);
        float rot = Center.GlobalRotation;
        WeaponNode.GlobalRotation = rot;
        Speed -= EXTRA_SPEED_WITHOUT_WEAPON;
        snarkPointer.Visible = false;
    }


    const float PLAYER_HIT_COOLDOWN = 1f;


    public override bool Hit(Vector2 knockback, int damage)
    {
        //player always gets damage of 1, regardless of enemy or explosion damage output
        damage = Mathf.Clamp(damage, -1, 1);
        if(base.Hit(knockback, damage)) {
            if(Health > 0 && damage > 0) {
                HitCooldown.Start(1f);
            }
            else if(Health <= 0) {
                DamageAnim.Stop();
            }
            UpdateStatsDisp();
            return true;
        }
        return false;
    }


    void TransferCamera() {
        camera.GetParent().RemoveChild(camera);
        LevelNode.AddChild(camera);
        camera.ParentNode = LevelNode;
        camera.GlobalPosition = GlobalPosition;
        camera.ShowRestart();
    }


    public void UpdateSlotsIcon() {
        loadout.UpdateSlotIcon(1);
        loadout.UpdateSlotIcon(2);
        loadout.UpdateSlotIcon(3);
        loadout.UpdateSlotIcon(4);
    }


    public void UpdateUpgrade() {
        loadout.UpdateUpgrade();
    }


    public void UpdateStatsDisp() {
        statsDesc.UpdateStatsDisp();
    }


    public void TriggerDialogue(Texture portrait, String name, Godot.Collections.Array stringArr,
    float portraitScale, bool show) {
        if(show) {
            hotkeyHUD.ShowDialogue(portrait, name, stringArr, portraitScale);
        }
        else {
            hotkeyHUD.HideDialogue();
        }
    }


}
