using Godot;
using System;

public class RigidCharger : BaseCharger
{
    const int CHARGE_ROTATION = 100;
    const int DEFAULT_DAMP = 15;
    const int MODIFIED_DAMP = 3;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        Mode = ModeEnum.Rigid;
        AngularVelocity = CHARGE_ROTATION;
        LinearDamp = MODIFIED_DAMP;
        base.Charge();
    }


    public override void FinishAction(String actionName) {
        Mode = ModeEnum.Character;
        LinearDamp = DEFAULT_DAMP;
        Rotation = 0;
        base.FinishAction(actionName);
    }



}
