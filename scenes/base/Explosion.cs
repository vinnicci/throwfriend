using Godot;
using System;

public abstract class Explosion : Area2D
{
    [Export] protected int damage = 1; 
    [Export] public int explosionRadius = 150;
    [Export] int cameraShakeIntensity = 10;
    [Export] float cameraShakeFrequency = 0.1f;
    [Export] float cameraShakeDuration = 0.2f;
    [Export] int cameraShakePriority = 0;
    public int ExplosionRadius {get; set;}
    public int Damage {get; set;}
    public Level LevelNode {get; set;}
    
    Polygon2D poly;
    CollisionShape2D collision;
    RayCast2D ray;
    AnimationPlayer anim;
    Tween tween;
    Particles2D particlesTop;
    Particles2D particlesBase;
    AudioStreamPlayer2D sound;
    const float CAM_DIST_MULT = 2;
    const int CIRCLE_PT_COUNT = 24;


    public override void _Ready()
    {
        base._Ready();
        Damage = damage;
        ExplosionRadius = explosionRadius;
        poly = (Polygon2D)GetNode("Polygon2D");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        UpdateRadius();
        ray = (RayCast2D)GetNode("RayCast2D");
        tween = (Tween)GetNode("Tween");
        anim = (AnimationPlayer)GetNode("Anim");
        particlesTop = (Particles2D)GetNode("ParticlesTop");
        particlesBase = (Particles2D)GetNode("ParticlesBase");
        sound = (AudioStreamPlayer2D)GetNode("Sound");
    }



    public override void _Process(float delta)
    {
        base._Process(delta);
        particlesBase.GlobalRotation = 0;
        particlesTop.GlobalRotation = 0;
    }


    const int KNOCKBACK = 250;
    const int BASE_PARTICLES_MULT = 5;
    const int TOP_PARTICLES_MULT = 10;
    static protected Godot.Collections.Dictionary calcDict = new Godot.Collections.Dictionary();


    public virtual bool Explode(bool detachSoundNode = true) {
        if(anim.IsPlaying()) {
            return false;
        }
        poly.Scale = new Vector2(1,1);
        String pAmountTop = STR_PARTICLES_AMOUNT_TOP + Filename;
        String pAmountBase = STR_PARTICLES_AMOUNT_BASE + Filename;
        String pCamDist = STR_CAM_DIST + Filename;
        //shake camera
        if(IsInstanceValid(LevelNode.PlayerNode) &&
        GlobalPosition.DistanceSquaredTo(LevelNode.PlayerNode.GlobalPosition) <= (int)calcDict[pCamDist]) {
            LevelNode.PlayerNode.Camera.ShakeCamera(new Vector2(cameraShakeIntensity, cameraShakeIntensity),
            cameraShakeFrequency, cameraShakeDuration, cameraShakePriority);
        }
        //reapply particles amount to avoid particle remnants
        particlesTop.Amount = (int)calcDict[pAmountTop];
        particlesBase.Amount = (int)calcDict[pAmountBase];
        particlesTop.ProcessMaterial.Set("emission_sphere_radius", ExplosionRadius);
        particlesBase.ProcessMaterial.Set("emission_sphere_radius", ExplosionRadius);
        particlesTop.Emitting = true;
        particlesBase.Emitting = true;
        //poly anim
        anim.Play("explode");
        tween.InterpolateProperty(poly, "scale", poly.Scale, poly.Scale*2, 0.75f, Tween.TransitionType.Linear,
        Tween.EaseType.InOut);
        tween.Start();
        //hit entities
        Godot.Collections.Array bodies = GetOverlappingBodies();
        foreach(Godot.Object body in bodies) {
            if(body == GetParent()) {
                continue;
            }
            if(body is Entity) {
                Entity entity = (Entity)body;
                ApplyDamage(entity);
            }
            else if(body is Weapon) {
                Weapon weap = (Weapon)body;
                ray.LookAt(weap.GlobalPosition);
                weap.ApplyCentralImpulse(new Vector2(KNOCKBACK, 0).Rotated(ray.GlobalRotation));
                weap.ContinuousCd = RigidBody2D.CCDMode.CastRay;
            }
        }
        Godot.Collections.Array areas = GetOverlappingAreas();
        foreach(Godot.Object area in areas) {
            if(((Area2D)area).Filename == "res://scenes/triggers/ExplosionDetector.tscn") {
                ((Trigger)area).OnSwitchedOn();
            }
        }
        //play sounds
        Node2D soundC = default;
        if(detachSoundNode) {
            soundC = (Node2D)sound.Duplicate();
            LeaveObj(soundC, 2f);
        }
        else {
            soundC = sound;
        }
        ((AudioStreamPlayer2D)soundC).Play();
        return true;
    }


    void LeaveObj(Node2D obj, float duration) {
        LevelNode.AddChild(obj);
        Tween tween = new Tween();
        obj.AddChild(tween);
        obj.GlobalPosition = GlobalPosition;
        obj.GlobalRotation = GlobalRotation;
        Godot.Collections.Array arr = new Godot.Collections.Array();
        arr.Add(obj);
        if(tween.IsConnected("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject)) == false) {
            tween.Connect("tween_all_completed", LevelNode, nameof(Level.QueueFreeObject), arr);
        }
        tween.InterpolateProperty(obj, "modulate", obj.Modulate, new Color(1,1,1,0), duration,
        Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }


    const String STR_PARTICLES_AMOUNT_TOP = "particlesAmountTop ";
    const String STR_PARTICLES_AMOUNT_BASE = "particlesAmountBase ";
    const String STR_CAM_DIST = "camDistSq ";



    public void UpdateRadius() {
        CircleShape2D circle = new CircleShape2D();
        circle.Radius = ExplosionRadius;
        collision.Shape = circle;
        Godot.Vector2[] circArr = new Vector2[CIRCLE_PT_COUNT];
        for(int i = 0; i <= CIRCLE_PT_COUNT - 1; i++) {
            Vector2 vec = new Vector2(ExplosionRadius,0).Rotated(Mathf.Deg2Rad(i*(360/CIRCLE_PT_COUNT)));
            circArr[i] = vec;
        }
        poly.Polygon = circArr;
        String pAmountTop = STR_PARTICLES_AMOUNT_TOP + Filename;
        UpdateExpValues(pAmountTop, ExplosionRadius * TOP_PARTICLES_MULT);
        String pAmountBase = STR_PARTICLES_AMOUNT_BASE + Filename;
        UpdateExpValues(pAmountBase, ExplosionRadius * BASE_PARTICLES_MULT);
        String pCamDist = STR_CAM_DIST + Filename;
        UpdateExpValues(pCamDist, (int)(ExplosionRadius*ExplosionRadius*CAM_DIST_MULT*CAM_DIST_MULT));
        float expScale = ExplosionRadius/explosionRadius;
        cameraShakeIntensity = (int)(cameraShakeIntensity*expScale);
        cameraShakeDuration *= expScale;
        if(expScale >= 2) {
            cameraShakePriority = 1;
            sound.VolumeDb = 5*expScale;
        }
        explosionRadius = ExplosionRadius;
    }


    void UpdateExpValues(String pName, int val) {
        if(calcDict.Contains(pName) == false) {
            calcDict.Add(pName, val);
        }
        else {
            calcDict[pName] = val;   
        }
    }


    public virtual void ApplyDamage(Entity en) {
        ray.LookAt(en.GlobalPosition);
        en.Hit(new Vector2(KNOCKBACK, 0).Rotated(ray.GlobalRotation), Damage);
    }


}
