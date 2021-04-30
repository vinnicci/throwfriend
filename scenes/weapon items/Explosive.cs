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
        WeaponNode.Connect("body_entered", this, "Explode");
    }


    private void Explode(Godot.Object body) {
        if(body is Player || WeaponNode.CurrentState != Weapon.States.ACTIVE) {
            return;
        }
        explosionNode.Damage = WeaponNode.Damage;
        explosionNode.Explode();
    }


    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }


}
