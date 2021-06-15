using Godot;
using System;

public class Explosive : WeaponItem
{
    private Explosion explosionNode;


    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Explosive");
    }


    public override void InitEffect()
    {
        base.InitEffect();
        explosionNode = (Explosion)GetNode("Explosion");
        if(WeaponNode.IsConnected("body_entered", this, "Explode") == false) {
            WeaponNode.Connect("body_entered", this, "Explode");
        }
    }


    private void Explode(Godot.Object body) {
        if(body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) {
            return;
        }
        explosionNode.Damage = WeaponNode.Damage * PlayerNode.SnarkDmgMult;
        if(WeaponNode.Filename == "res://scenes/player/WeaponLarge.tscn") {
            explosionNode.ExplosionRadius = explosionNode.explosionRadius * 2;
        }
        explosionNode.Explode();
    }


}
