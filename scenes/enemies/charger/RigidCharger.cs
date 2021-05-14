using Godot;
using System;

public class RigidCharger : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("charge", (float)actCooldown[0]);
    }


    private bool charging = false;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(charging == true && LinearVelocity.LengthSquared() <= 5625) {
            charging = false;
            SetCollisionMaskBit(Global.BIT_MASK_PLAYER, false);
        }
    }


    private const int CHARGE_KNOCKBACK = 100;
    private const int CHARGE_STRENGTH = 250;
    private const int CHARGE_ROTATION = 100;
    private const int DEFAULT_DAMP = 15;
    private const int MODIFIED_DAMP = 0;


    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
        if(charging == false) {
            return;
        }
        if(body == PlayerNode) {
            PlayerNode.Hit((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * CHARGE_KNOCKBACK, 1);
        }
    }


    public void Charge() {
        Sprite sprite = (Sprite)spriteChildren[0];
        if(sprite.FlipH == true) {
            foreach(Sprite spChild in spriteChildren) {
                spChild.Offset *= new Vector2(-1,1);
                spChild.FlipH = false;
            }
        }
        Mode = ModeEnum.Rigid;
        AngularVelocity = CHARGE_ROTATION;
        LinearDamp = MODIFIED_DAMP;
        ApplyCentralImpulse((PlayerNode.GlobalPosition - GlobalPosition).Clamped(1) * CHARGE_STRENGTH);
        charging = true;
        SetCollisionMaskBit(Global.BIT_MASK_PLAYER, true);
    }


    public override void FinishAction(String actionName) {
        Mode = ModeEnum.Character;
        LinearDamp = DEFAULT_DAMP;
        Rotation = 0;
        base.FinishAction(actionName);
    }



}
