using Godot;
using System;

public abstract class EnemyWeapon : Node2D, ISpawner, ISoundEmitter
{
    [Export] public Godot.Collections.Dictionary<String, PackedScene> spawnScenes {get; set;}
    [Export] Godot.Collections.Array<NodePath> bodies = new Godot.Collections.Array<NodePath>();
    
    public virtual Enemy ParentNode {get; set;}

    protected Particles2D spawnPoint;
    public AnimationPlayer Anim {get; set;}
    public Node2D SoundsNode {get; set;}


    public override void _Ready()
    {
        base._Ready();
        spawnPoint = (Particles2D)GetNode("Sprites/ProjSpawn");
        Anim = (AnimationPlayer)GetNode("Anim");
        Anim.Connect("animation_finished", this, nameof(FinishShooting));
        SoundsNode = (Node2D)GetNode("Sounds");
        SoundsDict = new Godot.Collections.Dictionary<String, AudioStreamPlayer2D>();
    }


    protected void EmitParticles() {
        spawnPoint.Amount = 50;
        spawnPoint.Emitting = true;
    }


    //generic melee action
    public virtual void MeleeAttack() {
        PlaySoundEffect("MeleeAttack");
        Anim.Play("melee_attack");
    }


    //generic ranged action
    public virtual void Shoot() {
        Anim.Play("shoot");
    }


    public virtual void SpawnInstance(String packedSceneKey, int count = 1) {
        if(packedSceneKey == "proj") {
            EmitParticles();
            PlaySoundEffect("Shoot");
            SpawnProj();
        }
    }


    //used by range attack anim to create projectiles
    public virtual void SpawnProj() {
        //EnemyProj proj = (EnemyProj)spawnScenes["proj"].Instance();
        EnemyProj proj = (EnemyProj)ParentNode.LevelNode.GetPooledObj(spawnScenes["proj"]);
        proj.AddHitException(ParentNode);
        proj.Spawn(ParentNode.LevelNode, spawnPoint.GlobalPosition, Vector2.Zero, spawnPoint.GlobalRotationDegrees);
    }


    public virtual void FinishShooting(String animName = "") {
        Anim.Play("idle");
    }


    public void Disable() {
        foreach(NodePath bodyPath in bodies) {
            Node2D body = (Node2D)GetNodeOrNull(bodyPath);
            if(body is Area2D) {
                Area2D area = (Area2D)body;
                area.SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
                area.SetCollisionMaskBit(Global.BIT_MASK_WEAPON, false);
            }
            else if(body is StaticBody2D) {
                StaticBody2D staticBody = (StaticBody2D)body;
                staticBody.SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
                staticBody.SetCollisionMaskBit(Global.BIT_MASK_WEAPON, false);
            }
        }
        Anim.Stop();
    }


    public Godot.Collections.Dictionary<String, AudioStreamPlayer2D> SoundsDict {get; set;}


    public void PlaySoundEffect(string soundName) {
        if(SoundsNode.HasNode(soundName) == false) {
            return;
        }
        if(SoundsDict.ContainsKey(soundName) == false) {
            SoundsDict.Add(soundName, (AudioStreamPlayer2D)SoundsNode.GetNode(soundName));
        }
        AudioStreamPlayer2D soundNode = SoundsDict[soundName];
        soundNode.Play();
    }


}
