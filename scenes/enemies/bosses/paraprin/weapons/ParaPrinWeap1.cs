using Godot;
using System;

public class ParaPrinWeap1 : EnemyWeapon
{
    public void MeleeAttackDir(bool up) {
        if(up == true) {
            anim.Play("melee_attack_up");
        }
        else {
            anim.Play("melee_attack_down");
        }
    }


    private const int KNOCKBACK = 250;


    protected void OnHitBoxBodyEntered(Godot.Object body) {
        IHealthModifiable player = (IHealthModifiable)body;
        player.Hit((((Node2D)player).GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
    }


}
