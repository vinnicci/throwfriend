using Godot;
using System;

public class BlobnadierShielded : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        InitAct("throw_blob", (float)actCooldown[0]);
    }


    public void ThrowBlob() {
        IBlobnade weap = (IBlobnade)WeaponNode;
        weap.ThrowBlob();
    }


    public override void FinishAction(String actionName) {
        base.FinishAction(actionName);
    }


}
