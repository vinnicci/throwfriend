using Godot;
using System;

public class EliteLongRangeCannon : EnemyWeapon
{
    EnemyWeapon[] weapArr = new EnemyWeapon[4];


    Enemy parentNode;
    public override Enemy ParentNode {
        get {
            return parentNode;
        }
        set {
            parentNode = value;
            foreach(EnemyWeapon weap in weapArr) {
                weap.ParentNode = ParentNode;
            }
        }
    }


    public override void _Ready()
    {
        base._Ready();
        weapArr[0] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon");
        weapArr[1] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon2");
        weapArr[2] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon3");
        weapArr[3] = (EnemyWeapon)GetNode("Sprites/EnemyWeapon4");
        weapArr[0].Anim.PlaybackSpeed = 1.5f;
        weapArr[1].Anim.PlaybackSpeed = 1.5f;
        weapArr[2].Anim.PlaybackSpeed = 1.5f;
        weapArr[3].Anim.PlaybackSpeed = 1.5f;
    }


    public void Shoot1() {
        Anim.Play("shoot_1");
    }


    public void Shoot2() {
        Anim.Play("shoot_2");
    }




}
