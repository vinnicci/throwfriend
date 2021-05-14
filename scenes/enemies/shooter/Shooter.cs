using Godot;
using System;

public class Shooter : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("shoot", (float)actCooldown[0]);
    }


    public void Shoot() {
        WeaponNode.Shoot();
    }


    public override void FinishAction(String actionName)
    {
        base.FinishAction(actionName);
    }


}
