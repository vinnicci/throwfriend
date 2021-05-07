using Godot;
using System;

public class Shooter : Enemy
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
    }


    public void Shoot() {
        WeaponNode.Shoot();
    }


    public override void FinishAction()
    {
        base.FinishAction();
    }


}
