using Godot;
using System;

public class Charger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }

    
}
