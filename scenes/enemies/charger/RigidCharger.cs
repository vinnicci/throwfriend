using Godot;
using System;

public class RigidCharger : BaseCharger
{
    private const int CHARGE_ROTATION = 100;
    private const int DEFAULT_DAMP = 15;
    private const int MODIFIED_DAMP = 0;


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