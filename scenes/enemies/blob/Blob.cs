using Godot;
using System;

public class Blob : Enemy
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
    }


    public override void FinishAction() {
        base.FinishAction();
    }


    public void Explode() {
        ExplosionNode.Explode();
    }


}
