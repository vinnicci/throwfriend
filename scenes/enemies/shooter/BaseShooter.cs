using Godot;
using System;

public abstract class BaseShooter : Enemy
{
    public void Shoot() {
        WeaponNode.Shoot();
    }


}
