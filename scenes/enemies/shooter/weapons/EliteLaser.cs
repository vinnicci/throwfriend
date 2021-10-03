using Godot;
using System;

public class EliteLaser : EnemyWeapon
{
    EnemyWeapon[] weapArr = new EnemyWeapon[2];


    Enemy parentNode;
    public override Enemy ParentNode {
        get {
            return parentNode;
        }
        set {
            parentNode = value;
            weapArr[0].ParentNode = ParentNode;
            weapArr[1].ParentNode = ParentNode;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        weapArr[0] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon");
        weapArr[1] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon2");
        weapArr[0].Anim.PlaybackSpeed = 1.5f;
        weapArr[1].Anim.PlaybackSpeed = 1.5f;
    }


    public void Shoot1() {
        Anim.Play("shoot_1");
    }


    public void Shoot2() {
        if(GD.RandRange(0, 1f) <= 0.5f) {
            Anim.Play("shoot_2_1");
        }
        else {
            Anim.Play("shoot_2_2");
        }
    }



}
