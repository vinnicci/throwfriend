using Godot;
using System;

public class Explosive : WeaponItem
{
    public Explosion ExplosionNode {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        ExplosionNode = (Explosion)GetNode("Explosion");
    }


    public override void InitEffect()
    {
        base.InitEffect();
        if(this == WeaponNode.Item2 && WeaponNode.Item1 is Explosive) {
            ((Explosive)WeaponNode.Item1).ExplosionNode.ExplosionRadius *= 2;
        }
        if(WeaponNode.IsConnected("body_entered", this, nameof(Explode)) == false) {
            WeaponNode.Connect("body_entered", this, nameof(Explode));
        }
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Explode(null);
    }


    void Explode(Godot.Object body) {
        if(IsInstanceValid(body) && (body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) ||
        (this == WeaponNode.Item2 && WeaponNode.Item1 is Explosive)) {
            return;
        }
        if(IsInstanceValid(PlayerNode) == false) {
            PlayerNode = WeaponNode.PlayerNode;
        }
        ExplosionNode.Damage = WeaponNode.Damage * PlayerNode.SnarkDmgMult;
        ExplosionNode.Explode();
    }


}
