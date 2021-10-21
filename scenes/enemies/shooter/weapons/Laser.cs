using Godot;
using System;

public class Laser : EnemyWeapon
{
    const int KNOCKBACK = 250;


    public override void Shoot() {
        base.Shoot();
        PlaySoundEffect("Shoot");
    }


    void OnBeamBodyEntered(Godot.Object body) {
        if(body is Player) {
            Player player = (Player)body;
            player.Hit((player.GlobalPosition - ParentNode.GlobalPosition).Clamped(1) * KNOCKBACK, 1);
        }
    }


}
