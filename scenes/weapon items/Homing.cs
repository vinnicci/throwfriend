using Godot;
using System;
using System.Collections.Generic;

public class Homing : WeaponItem
{
    Area2D range;
    RayCast2D ray;
    Timer tick;
    Enemy target;


    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Homing");
        incompatibilityList.Add("Guided");
        range = (Area2D)GetNode("DetectionRange");
        ray = (RayCast2D)GetNode("DetectionRay");
        tick = (Timer)GetNode("Tick");
    }


    const int HOME_MAGNITUDE = 150;
    const int HOME_MAGNITUDE_CLOSE = 500;
    const int DIST_HOME_ACCEL = 2500;
    Queue<Enemy> enemies = new Queue<Enemy>();


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(IsInstanceValid(WeaponNode) == false) {
            return;
        }
        if(WeaponNode.CurrentState == Weapon.States.HELD) {
            if(enemies.Count > 0) {
                enemies.Clear();
            }
            if(IsInstanceValid(target) == true) {
                target = null;
            }
        }
        else if(WeaponNode.CurrentState == Weapon.States.ACTIVE) {
            if(IsInstanceValid(target) == true && target.IsDead == false) {
                int mag;
                if(WeaponNode.GlobalPosition.DistanceSquaredTo(target.GlobalPosition) <= DIST_HOME_ACCEL) {
                    mag = HOME_MAGNITUDE_CLOSE;
                }
                else {
                    mag = HOME_MAGNITUDE;
                }
                Vector2 vec = (target.Position - WeaponNode.Position).Clamped(1) * mag;
                WeaponNode.Velocity += vec;
            }
        }
    }


    void OnDetectionRangeBodyEntered(Godot.Object body) {
        if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            if(enemies.Contains(enemy) == false && enemy.HasNode("AI") == true) {
                enemies.Enqueue(enemy);
                if(tick.IsStopped() == true) {
                    tick.Start();
                }
            }
        }
    }


    void OnTickTimeout() {
        if(enemies.Count == 0) {
            return;
        }
        Enemy enemy = enemies.Dequeue();
        if((IsInstanceValid(target) == true && target.IsDead == false) ||
        (IsInstanceValid(enemy) == false || enemy.IsDead == true)) {
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
