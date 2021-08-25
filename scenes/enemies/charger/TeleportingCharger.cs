using Godot;
using System;

public class TeleportingCharger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body)
    {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public void TeleCharge() {
        Vector2 vec = LevelNode.GetPlayerPos() -
        new Vector2(50,0).Rotated(Godot.Mathf.Deg2Rad((float)GD.RandRange(0, 360)));
        Teleport(LevelNode, vec);
    }


}
