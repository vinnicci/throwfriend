using Godot;
using System;

public class Sword : EnemyWeapon
{
    private const int KNOCKBACK = 500;


    private void OnHitboxBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


    public override void FinishShooting()
    {
        base.FinishShooting();
    }


}
