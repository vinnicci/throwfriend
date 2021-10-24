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
        incompatibilityList.Add("AutoRetrieve");
        range = (Area2D)GetNode("DetectionRange");
        ray = (RayCast2D)GetNode("DetectionRay");
        tick = (Timer)GetNode("Tick");
        turretAIs.Add("res://scenes/enemies/all rounder/ais/TurretAllRounderAI.tscn");
        turretAIs.Add("res://scenes/enemies/shooter/ais/TurretShooterAI.tscn");
    }


    const int HOME_MAGNITUDE = 250;
    const int HOME_MAGNITUDE_CLOSE = 750;
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
            if(IsInstanceValid(target)) {
                target = null;
            }
        }
        else if(WeaponNode.CurrentState == Weapon.States.ACTIVE) {
            if(IsInstanceValid(target) && target.Health > 0) {
                int mag;
                if(WeaponNode.GlobalPosition.DistanceSquaredTo(target.GlobalPosition) <= DIST_HOME_ACCEL) {
                    mag = HOME_MAGNITUDE_CLOSE;
                }
                else {
                    mag = HOME_MAGNITUDE;
                }
                Vector2 vec = (target.Position - WeaponNode.Position).Normalized() * mag;
                WeaponNode.Velocity += vec;
            }
        }
    }


    Godot.Collections.Array turretAIs = new Godot.Collections.Array();


    void OnDetectionRangeBodyEntered(Godot.Object body) {
        if(body is Enemy) {
            Enemy enemy = (Enemy)body;
            //won't home to turret enemies
            if(enemies.Contains(enemy) == false && enemy.HasNode("AI") &&
            turretAIs.Contains(enemy.GetNode("AI").Filename) == false) {
                enemies.Enqueue(enemy);
                if(tick.IsStopped()) {
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
        if((IsInstanceValid(target) && target.Health > 0) ||
        (IsInstanceValid(enemy) == false || enemy.Health <= 0)) {
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
