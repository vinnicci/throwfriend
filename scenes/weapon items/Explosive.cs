using Godot;
using System;

public class Explosive : WeaponItem
{
    private Explosion explosionNode;


    public override void _Ready()
    {
        incompatibilityList.Add("res://scenes/weapon items/Explosive.cs");
    }


    public override void InitEffect()
    {
        base.InitEffect();
        explosionNode = (Explosion)GetNode("Explosion");
        WeaponNode.Connect("body_entered", this, "Explode");
    }


    private void Explode(Godot.Object body) {
        if(body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) {
            return;
        }
        ApplyEffect();
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        explosionNode.Damage = WeaponNode.Damage;
        explosionNode.Explode();
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }


}