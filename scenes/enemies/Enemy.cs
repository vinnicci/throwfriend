using Godot;
using System;

public abstract class Enemy : Entity, ISpawner, ILevelObject
{
    [Export] protected Godot.Collections.Dictionary acts =
    new Godot.Collections.Dictionary();
    [Export] protected Godot.Collections.Array<int> patrolPoints =
    new Godot.Collections.Array<int>();
    [Export] public Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}
    [Export] public bool Persist {get; set;}

    public override Level LevelNode {
        get {
            return levelNode;
        }
        set{
            base.LevelNode = value;
            if(HasNode("EnemyWeapon")) {
                WeaponNode = (EnemyWeapon)GetNode("EnemyWeapon");
                WeaponNode.ParentNode = this;
            }
            //override AI
            if(HasNode("AI2")) {
                Node2D tempAI = (Node2D)GetNode("AI");
                tempAI.GetParent().RemoveChild(tempAI);
                tempAI.QueueFree();
                Node2D newAI = (Node2D)GetNode("AI2");
                newAI.Name = "AI";
            }
            if(HasNode("AI")) {
                aINode = (Node2D)GetNode("AI");
                aINode.Call("init_properties", levelNode, this, patrolPoints);
            }
            ChangeEntityBaseStats((int)(health*LevelNode.enemyHealthMult), (int)(speed*LevelNode.enemySpeedMult));
        }
    }
    public EnemyWeapon WeaponNode {get; protected set;}
    public String SwitchedOnSignal {get; set;}
    [Export] public Godot.Collections.Array<NodePath> BoundTriggers {get; set;}
    
    //not implemented
    public AnimationPlayer TriggerAnim {get; set;}
    public String SwitchedOffSignal {get; set;}
    
    protected Node2D aINode;
    protected Explosion ExplosionNode {get; private set;}    
    protected Godot.Collections.Dictionary ActDict {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        if(HasNode("Explosion")) {
            ExplosionNode = (Explosion)GetNode("Explosion");
        }
        ActDict = new Godot.Collections.Dictionary();
        foreach(String act in acts.Keys) {
            InitAct(act, (float)((Godot.Collections.Array)acts[act])[0],
            (float)((Godot.Collections.Array)acts[act])[1]);
        }
        anim.Connect("animation_finished", this, nameof(FinishAction));
        InitLevelObject();
    }


    [Signal] public delegate void SwitchedOn();
    [Signal] public delegate void SwitchedOff();



    public void InitLevelObject() {
        SwitchedOnSignal = nameof(SwitchedOn);
        SwitchedOffSignal = nameof(SwitchedOff);
        foreach(NodePath nodePath in BoundTriggers) {
            Node2D node = GetNodeOrNull<Node2D>(nodePath);
            if(IsInstanceValid(node) == false ||
            node.IsConnected("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers))) {
                continue;
            }
            Godot.Collections.Array arr = new Godot.Collections.Array();
            arr.Add(nodePath);
            arr.Add(true);
            node.Connect("SwitchedOn", this, nameof(OnTriggeredAllBoundTriggers), arr);
        }
    }


    public void OnSwitchedOn() {
        if(Health > 0) {
            Die();
        }
    }


    public void OnSwitchedOff() {}


    public void OnAnimFinished(String animName) {
        Global.PrintErrNotImplemented(GetType().ToString(), nameof(OnAnimFinished));
    }


    public void OnTriggeredAllBoundTriggers(NodePath path, bool triggered) {
        if(Persist && BoundTriggers.Count == 0) {
            return;
        }
        if(triggered) {
            BoundTriggers.Remove(path);
        }
        else {
            BoundTriggers.Add(path);            
        }
        if(BoundTriggers.Count == 0) {
            OnSwitchedOn();
        }
    }


    protected void InitAct(String actionName, float length, float hpPercent) {
        Godot.Collections.Dictionary act = new Godot.Collections.Dictionary();
        act["IsActive"] = false;
        act["TimeRemain"] = 0f;
        act["Length"] = length;
        act["HPPercent"] = hpPercent;
        ActDict[actionName] = act;
    }


    public bool IsActReady(String actionName) {
        Godot.Collections.Array keys = (Godot.Collections.Array)ActDict.Keys;
        if(keys.Contains(actionName) == false) {
            return false;
        }
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        return (bool)action["IsActive"] == false && //action not animating
            (float)action["TimeRemain"] <= 0 && //cooling down
            Health <= health*(float)action["HPPercent"]; //hp requirement 
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        foreach(String actionName in ActDict.Keys) {
            Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
            float timeRemain = (float)action["TimeRemain"];
            if(timeRemain > 0f) {
                timeRemain -= delta;
                action["TimeRemain"] = timeRemain;
            }
        }
    }


    public virtual void OnEnemyBodyEntered(Godot.Object body) {
        if(Health <= 0) {
            LevelNode.PlayerEngaging.Remove(Name);
            GD.Print("die: " + LevelNode.PlayerEngaging.Count);
        }
        else if(body is Weapon && IsInstanceValid(aINode) &&
        IsInstanceValid((Entity)((Godot.Collections.Dictionary)aINode.Get("bb"))["enemy"]) == false) {
            Weapon weap = (Weapon)body;
            aINode.Call("engage_enemy", weap.PlayerNode);
        }
    }


    public virtual bool DoAction(String actionName) {
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        if(Health <= 0 || IsActReady(actionName) == false) {
            return false;
        }
        action["IsActive"] = true;
        anim.Stop();
        anim.Play(actionName);
        return true;
    }


    const float CHANCE_HP_DROP = 10f;


    public override bool Hit(Vector2 knockback, int damage)
    {
        if(base.Hit(knockback, damage)) {
            if(Health <= 0) {
                if(IsInstanceValid(WeaponNode)) {
                    WeaponNode.Disable();
                }
                if(GD.RandRange(0, 100) <= CHANCE_HP_DROP) {
                    CallDeferred(nameof(SpawnInstance), "hp_drop", 1);
                }
                EmitSignal(nameof(SwitchedOn));
            }
            return true;
        }
        return false;
    }


    public override void Die()
    {
        base.Die();
        if(IsInstanceValid(aINode)) {
            Godot.Collections.Dictionary bbDict = (Godot.Collections.Dictionary)aINode.Get("bb");
            if(IsInstanceValid((RigidBody2D)bbDict["enemy"])) {
                LevelNode.PlayerEngaging.Remove(Name);
                GD.Print("die: " + LevelNode.PlayerEngaging.Count);
            }
        }
    }


    public virtual void SpawnInstance(String packedSceneKey, int count = 1) {
        if(packedSceneKey == "hp_drop") {
            HealthPickup healthInst = (HealthPickup)spawnScenes[packedSceneKey].Instance();
            healthInst.Spawn(LevelNode, GlobalPosition, Vector2.Zero);
        }
    }


    //override and use this func to end action anims
    public virtual void FinishAction(String actionName) {
        if(ActDict.Contains(actionName) == false) {
            return;
        }
        Godot.Collections.Dictionary action = (Godot.Collections.Dictionary)ActDict[actionName];
        action["IsActive"] = false;
        action["TimeRemain"] = action["Length"];
        anim.Play("idle");
    }


}
