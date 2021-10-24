using Godot;
using System;

public class RigidCharger : BaseCharger
{
    const int CHARGE_ROTATION = 100;
    const int DEFAULT_DAMP = 15;
    const int MODIFIED_DAMP = 3;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
        if(charging) {
            PlaySoundEffect("ChargeCollide");
        }
    }


    Vector2 toCharge;


    void SetChargeDest() {
        toCharge = LevelNode.GetPlayerPos();
    }


    public override void Charge() {
        //rigid mode
        Mode = ModeEnum.Rigid;
        AngularVelocity = CHARGE_ROTATION;
        LinearDamp = MODIFIED_DAMP;
        //base exec
        Vector2 vec = toCharge;
        if(vec == Vector2.Zero) {
            return;
        }
        ApplyCentralImpulse((vec - GlobalPosition).Clamped(1) * CHARGE_STRENGTH);
        ContinuousCd = RigidBody2D.CCDMode.CastRay;
        charging = true;
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
    }


    public override void FinishAction(String actionName) {
        Mode = ModeEnum.Character;
        LinearDamp = DEFAULT_DAMP;
        Rotation = 0;
        base.FinishAction(actionName);
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }



}
