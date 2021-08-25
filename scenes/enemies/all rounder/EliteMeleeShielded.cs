using Godot;
using System;

public class EliteMeleeShielded : AllRounder
{
    const String AI_PATH = "res://scenes/enemies/all rounder/ais/EliteMeleeShieldedAI.tscn";


    //replace ai
    public override void _Ready()
    {
        base._Ready();
        Node2D tempAI = GetNode<Node2D>("AI");
        tempAI.GetParent().RemoveChild(tempAI);
        tempAI.QueueFree();
        PackedScene aIPack = (PackedScene)ResourceLoader.Load(AI_PATH);
        aINode = (Node2D)aIPack.Instance();
        AddChild(aINode);
    }


    public override void MeleeAttackBack() {
        base.MeleeAttackBack();
    }


    public override void AttackImpulse()
    {
        base.AttackImpulse();
    }


    public void TeleAttack() {
        Vector2 vec = LevelNode.GetPlayerPos() -
        new Vector2(50,0).Rotated(Godot.Mathf.Deg2Rad((float)GD.RandRange(0, 360)));
        Teleport(LevelNode, vec);
    }


    public void ThrowFlyBlob() {
        ((EliteSwordAndShield)WeaponNode).ThrowFlyBlob();
    }


}
