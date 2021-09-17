using Godot;
using System;

public abstract class Explosion : Area2D
{
    [Export] protected int damage = 1; 
    [Export] public int explosionRadius = 150;
    public int ExplosionRadius {get; set;}
    public int Damage {get; set;}
    
    Polygon2D poly;
    CollisionShape2D collision;
    RayCast2D ray;
    AnimationPlayer anim;


    public override void _Ready()
    {
        base._Ready();
        poly = (Polygon2D)GetNode("Polygon2D");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        ray = (RayCast2D)GetNode("RayCast2D");
        anim = (AnimationPlayer)GetNode("Anim");
        Damage = damage;
        ExplosionRadius = explosionRadius;
        CircleShape2D circle = new CircleShape2D();
        circle.Radius = ExplosionRadius;
        collision.Shape = circle;
        int circleCountPtCount = 24;
        Godot.Vector2[] circArr = new Vector2[circleCountPtCount];
        for(int i = 0; i <= circleCountPtCount - 1; i++) {
            Vector2 vec = new Vector2(ExplosionRadius,0).Rotated(Mathf.Deg2Rad(i*(360/circleCountPtCount)));
            circArr[i] = vec;
        }
        poly.Polygon = circArr;
    }


    const int KNOCKBACK = 250;


    public virtual bool Explode() {
        if(anim.IsPlaying()) {
            return false;
        }
        if(explosionRadius != ExplosionRadius) {
            float scale = ExplosionRadius/explosionRadius;
            collision.Scale = new Vector2(1,1)*scale;
            poly.Scale = new Vector2(1,1)*scale;
        }
        anim.Play("explode");
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
        return true;
    }


    public virtual void ApplyDamage(Entity en) {
        ray.LookAt(en.GlobalPosition);
        en.Hit(new Vector2(KNOCKBACK, 0).Rotated(ray.GlobalRotation), Damage);
    }


}
