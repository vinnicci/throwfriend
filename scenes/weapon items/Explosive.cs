using Godot;
using System;

public class Explosive : WeaponItem
{
    Explosion explosionNode;


    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("Explosive");
    }


    public override void InitEffect()
    {
        base.InitEffect();
        explosionNode = (Explosion)GetNode("Explosion");
        if(WeaponNode.IsConnected("body_entered", this, nameof(Explode)) == false) {
            WeaponNode.Connect("body_entered", this, nameof(Explode));
        }
    }


    void Explode(Godot.Object body) {
        if(body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) {
            return;
        }
        explosionNode.Damage = WeaponNode.Damage * PlayerNode.SnarkDmgMult;
        if(WeaponNode.Filename == Global.WEAP_LARGE_FILE) {
            explosionNode.ExplosionRadius = explosionNode.explosionRadius * 2;
        }
        explosionNode.Explode();
    }


}
