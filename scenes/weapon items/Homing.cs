using Godot;
using System;
using System.Collections.Generic;

public class Homing : WeaponItem
{
    private Area2D range;
    private RayCast2D ray;
    private Timer tick;
    private Enemy target;


    public override void _Ready()
    {
        range = (Area2D)GetNode("DetectionRange");
        ray = (RayCast2D)GetNode("DetectionRay");
        tick = (Timer)GetNode("Tick");
    }


    private const int HOME_MAGNITUDE = 250;
    private Queue<Enemy> enemies = new Queue<Enemy>();


    public override void _PhysicsProcess(float delta)
    {
        if(Godot.Object.IsInstanceValid(target) == false) {
            return;
        }
        if(WeaponNode.IsTakable == true) {
            if(enemies.Count > 0) {
                enemies.Clear();
                target = null;
            }
        }
        else if(WeaponNode.IsTakable == false) {
            if(WeaponNode.Mode == RigidBody2D.ModeEnum.Rigid) {
                Vector2 vec = (target.Position - WeaponNode.Position).Clamped(1) * HOME_MAGNITUDE;
                WeaponNode.Velocity += vec;
            }
        }
    }


    private void OnDetectionRangeBodyEntered(Godot.Object body) {
        if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            if(enemies.Contains(enemy) == false) {
                enemies.Enqueue(enemy);
                if(tick.IsStopped() == true) {
                    tick.Start();
                }
            }
        }
    }


    private void OnTickTimeout() {
        if(enemies.Count == 0) {
            return;
        }
        Enemy enemy = enemies.Dequeue();
        if(Godot.Object.IsInstanceValid(target) == true || Godot.Object.IsInstanceValid(enemy) == false) {
            return;
        }
        ray.LookAt(enemy.GlobalPosition);
        ray.ForceRaycastUpdate();
        if(ray.GetCollider() == enemy) {
            target = enemy;
        }
        else {
            enemies.Enqueue(enemy);
        }
    }


}
