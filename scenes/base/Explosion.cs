using Godot;
using System;

public class Explosion : Area2D
{
    [Export] protected int explosionRadius = 150;
    public int ExplosionRadius {get; set;}
    public Node2D ParentNode {get; set;}
    public int Damage {get; set;}
    
    private Polygon2D poly;
    private CollisionShape2D collision;
    private RayCast2D ray;
    private AnimationPlayer anim;

    
    public override void _Ready()
    {
        poly = (Polygon2D)GetNode("Polygon2D");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        ray = (RayCast2D)GetNode("RayCast2D");
        anim = (AnimationPlayer)GetNode("Anim");
        ExplosionRadius = explosionRadius;
        CircleShape2D circle = new CircleShape2D();
        circle.Radius = ExplosionRadius;
        collision.Shape = circle;
        Godot.Vector2[] circArr = new Vector2[24];
        for(int i = 0; i<=23; i++) {
            Vector2 vec = new Vector2(ExplosionRadius,0).Rotated(Godot.Mathf.Deg2Rad(i*15));
            circArr[i] = vec;
        }
        poly.Polygon = circArr;
    }


    private const int KNOCKBACK = 400;


    public void Explode() {
        if(anim.IsPlaying() == true) {
            return;
        }
        anim.Play("explode");
        Godot.Collections.Array bodies = GetOverlappingBodies();
        foreach(Godot.Object body in bodies) {
            if(body is Entity) {
                Entity entity = (Entity)body;
                ray.LookAt(entity.GlobalPosition);
                int dmg;
                if(entity is Player) {
                    dmg = 0;
                }
                else {
                    dmg = Damage;
                }
                entity.Hit(new Vector2(KNOCKBACK, 0).Rotated(ray.GlobalRotation), dmg);
            }
        }
    }


}
