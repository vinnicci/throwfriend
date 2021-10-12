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
            ((Explosive)WeaponNode.Item1).ExplosionNode.UpdateRadius();
        }
        if(WeaponNode.IsConnected("body_entered", this, nameof(Explode)) == false) {
            WeaponNode.Connect("body_entered", this, nameof(Explode));
        }
        ExplosionNode.LevelNode = PlayerNode.LevelNode;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Switch(true, Active == false);
    }


    public override void Switch(bool thisInst, bool active) {
        base.Switch(thisInst, active);
        if(thisInst == false) {
            return;
        }
        else if(this == WeaponNode.Item1 && WeaponNode.Item2 is Explosive) {
            ((Explosive)WeaponNode.Item2).Switch(false, active);
        }
        else if(this == WeaponNode.Item2 && WeaponNode.Item1 is Explosive) {
            ((Explosive)WeaponNode.Item1).Switch(false, active);
        }
    }


    public void Explode(Godot.Object body) {
        if(Active == false ||
        IsInstanceValid(body) && (body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) ||
        (this == WeaponNode.Item2 && WeaponNode.Item1 is Explosive)) {
            return;
        }
        if(IsInstanceValid(PlayerNode) == false) {
            PlayerNode = WeaponNode.PlayerNode;
        }
        ExplosionNode.Damage = PlayerNode.SnarkDmg * PlayerNode.SnarkDmgMult;
        ExplosionNode.Explode();
    }


}
