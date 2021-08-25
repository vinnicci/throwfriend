using Godot;
using System;

public class EliteRangedShielded : AllRounder
{
    const String AI_PATH = "res://scenes/enemies/all rounder/ais/EliteRangedShieldedAI.tscn";


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


    public override void ShootBack() {
        base.ShootBack();
    }


    public override void ThrowBlob()
    {
        base.ThrowBlob();
    }


    public override void Recoil() {
        base.Recoil();
    }


    public void SuperScatter() {
        ((EliteBlasterAndShield)WeaponNode).SuperScatter();
    }


    public void TripleShot() {
        ((EliteBlasterAndShield)WeaponNode).TripleShot();
    }


}
