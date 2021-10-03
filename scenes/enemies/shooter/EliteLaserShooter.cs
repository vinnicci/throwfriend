using Godot;
using System;

public class EliteLaserShooter : BaseShooter
{
    Tween tween;


    public override void _Ready()
    {
        base._Ready();
        tween = (Tween)GetNode("Anims/Tween");
    }


    public void AimToEnemy() {
        if(IsInstanceValid(LevelNode.PlayerNode) == false) {
            return;
        }
        tween.InterpolateProperty(WeaponNode, "global_rotation", WeaponNode.GlobalRotation,
        (LevelNode.PlayerNode.GlobalPosition - WeaponNode.GlobalPosition).Angle(), 0.1f, Tween.TransitionType.Linear,
        Tween.EaseType.InOut);
        tween.Start();
    }


    public void Shoot1() {
        ((EliteLaser)WeaponNode).Shoot1();
    }


    public void Shoot2() {
        ((EliteLaser)WeaponNode).Shoot2();
    }


}
