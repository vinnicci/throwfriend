using Godot;
using System;

public class Swap : PlayerItem
{
    const float FAILURE_CHANCE = 5;
    Area2D attackRange;


    public override void _Ready()
    {
        base._Ready();
        attackRange = (Area2D)GetNode("AttackRange");
    }


    public override void ApplyEffect()
    {
        if(WeaponNode.CurrentState == Weapon.States.HELD || Cooldown.IsStopped() == false ||
        PlayerNode.TeleportAnim.IsPlaying() || WeaponNode.TeleportAnim.IsPlaying()) {
            return;
        }
        base.ApplyEffect();
        EmitSignal(nameof(Activated), Cooldown.WaitTime);
        Cooldown.Start();
        if(GD.RandRange(0, 100f) <= FAILURE_CHANCE) {
            return;
        }
        var playerPos = PlayerNode.GlobalPosition;
        PlayerNode.Teleport(PlayerNode.LevelNode, WeaponNode.GlobalPosition);
        WeaponNode = PlayerNode.WeaponNode;
        WeaponNode.Teleport(PlayerNode.LevelNode, playerPos);
        if(WeaponNode.Item1 is SplitToThree) {
            TeleportClones((SplitToThree)WeaponNode.Item1, playerPos);
        }
        if(WeaponNode.Item2 is SplitToThree) {
            TeleportClones((SplitToThree)WeaponNode.Item2, playerPos);
        }
        attackStep = 1;
        SetPhysicsProcess(true);
    }


    void TeleportClones(SplitToThree item, Vector2 playerPos) {
        Weapon w = item.Weaps[0];
        w.Teleport(PlayerNode.LevelNode, playerPos);
        w = item.Weaps[1];
        w.Teleport(PlayerNode.LevelNode, playerPos);
    }


    int attackStep = 0;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        //using attack steps up to 2 because area2d GetOverlappingBodies func only gets new bodies on physics step
        switch(attackStep) {
            case 0: SetPhysicsProcess(false); break;
            case 1: attackStep = 2; break;
            case 2: {
                attackStep = 0;
                Attack();
            } break;
        }
    }


    void Attack() {
        Godot.Collections.Array arr = attackRange.GetOverlappingBodies();
        if(arr.Count <= 0) {
            return;
        }
        arr.Shuffle();
        WeaponNode.LookAt(((Enemy)arr[0]).GlobalPosition);
        WeaponNode.Throw(PlayerNode.ThrowStrength, WeaponNode.GlobalPosition, Vector2.Zero, WeaponNode.GlobalRotation);
        if(WeaponNode.Item1 is SplitToThree) {
            ApplyAttackToClones((SplitToThree)WeaponNode.Item1, arr);
        }
        if(WeaponNode.Item2 is SplitToThree) {
            ApplyAttackToClones((SplitToThree)WeaponNode.Item2, arr);
        }
    }


    void ApplyAttackToClones(SplitToThree item, Godot.Collections.Array arr) {
        Weapon w = item.Weaps[0];
        arr.Shuffle();
        w.LookAt(((Enemy)arr[0]).GlobalPosition);
        w.Throw(PlayerNode.ThrowStrength, w.GlobalPosition, Vector2.Zero, w.GlobalRotation);
        w = item.Weaps[1];
        arr.Shuffle();
        w.LookAt(((Enemy)arr[0]).GlobalPosition);
        w.Throw(PlayerNode.ThrowStrength, w.GlobalPosition, Vector2.Zero, w.GlobalRotation);
    }


}
