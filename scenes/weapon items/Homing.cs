using Godot;
using System;
using System.Collections.Generic;

public class Homing : WeaponItem
{
    Area2D range;
    RayCast2D ray;
    Timer tick;
    Enemy target;
    Sprite currentTargetIndicator;


    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Homing");
        range = (Area2D)GetNode("DetectionRange");
        ray = (RayCast2D)GetNode("DetectionRay");
        tick = (Timer)GetNode("Tick");
        currentTargetIndicator = (Sprite)GetNode("TargetIndicator");
        turretAIs.Add("res://scenes/enemies/all rounder/ais/TurretAllRounderAI.tscn");
        turretAIs.Add("res://scenes/enemies/shooter/ais/TurretShooterAI.tscn");
    }


    const int HOME_MAGNITUDE = 125;
    const int HOME_MAGNITUDE_CLOSE = 625;
    const int ENEMY_CLOSE_DIST = 5625;
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
                if(WeaponNode.GlobalPosition.DistanceSquaredTo(target.GlobalPosition) <= ENEMY_CLOSE_DIST) {
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


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(IsInstanceValid(WeaponNode) == false) {
            return;
        }
        if(IsInstanceValid(target) == false) {
            if(currentTargetIndicator.Visible) {
                currentTargetIndicator.Visible = false;
            }
            return;
        }
        else if(IsInstanceValid(target)) {
            if(WeaponNode.CurrentState == Weapon.States.ACTIVE) {
                if(currentTargetIndicator.Visible == false) {
                    currentTargetIndicator.Visible = true;
                }
                currentTargetIndicator.GlobalPosition = target.GlobalPosition;
                currentTargetIndicator.GlobalRotation = 0;
            }
            else {
                currentTargetIndicator.Visible = false;
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
            currentTargetIndicator.GlobalPosition = target.GlobalPosition;
        }
        else {
            enemies.Enqueue(enemy);
        }
    }


}
