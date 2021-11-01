using Godot;
using System;
using System.Text.RegularExpressions;

public class TeleportToWeapon : PlayerItem
{
    const float FAILURE_CHANCE = 5;
    RayCast2D ray;


    public override void _Ready()
    {
        base._Ready();
        ray = (RayCast2D)GetNode("RayCast2D");
        SetProcess(false);
    }


    public override void ApplyEffect()
    {
        if(Cooldown.IsStopped() == false || PlayerNode.TeleportAnim.IsPlaying() ||
        WeaponNode.TeleportAnim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        holdCountdown = TELEPORT_COUNTDOWN;
        if(currentSlot == "") {
            currentSlot = GetCurrentSlot();
        }
        SetProcess(true);
    }


    String GetCurrentSlot() {
        String[] patternStr = {
            "ItemSlot1", "ItemSlot2"
        };
        int i = 0;
        foreach(String pattern in patternStr) {
            i++;
            Regex regex = new Regex(pattern);
            String path = GetParent().GetPath().ToString();
            if(regex.IsMatch(path)) {
                return "hotkey_" + i; 
            }
        }
        return default;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Cooldown.IsStopped()) {
            ray.CastTo = GetLocalMousePosition();
            if(ray.Enabled == false) {
                ray.Enabled = true;
            }
        }
        else if(Cooldown.IsStopped() == false && ray.Enabled) {
            ray.Enabled = false;
        }
    }


    const float TELEPORT_COUNTDOWN = 0.3f;
    float holdCountdown = 0;
    String currentSlot = "";


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(holdCountdown <= 0) {
            return;
        }
        else if(holdCountdown > 0 && Input.IsActionPressed(currentSlot)) {
            holdCountdown -= delta;
        }
        if(Input.IsActionJustReleased(currentSlot) && holdCountdown > 0) {
            //quick press, teleport to cursor
            TeleportToCursor();
            holdCountdown = 0;
            SetProcess(false);
        }
        else if(holdCountdown <= 0) {
            //hold key long enough, teleport to snark
            TeleportToSnark();
            holdCountdown = 0;
            SetProcess(false);
        }
    }


    void TeleportToCursor() {
        if(ray.IsColliding()) {
            return;
        }
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) > FAILURE_CHANCE) {
            PlayerNode.Teleport(PlayerNode.LevelNode, GetGlobalMousePosition());
        }
        else {
            PlaySoundEffect("TeleportFailed");
        }
    }


    void TeleportToSnark() {
        if(WeaponNode.Mode != RigidBody2D.ModeEnum.Rigid) {
            return;
        }
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) > FAILURE_CHANCE) {
            PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        }
        else {
            PlaySoundEffect("TeleportFailed");
        }
    }


}
