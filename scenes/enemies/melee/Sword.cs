using Godot;
using System;

public class Sword : EnemyWeapon
{
    private const int KNOCKBACK = 500;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(ParentNode.IsDead == true && anim.IsPlaying() == true) {
            anim.Stop();
            QueueFree();
        }
    }


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
