using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    bool weapIsReturning = false;
    const int RETRIEVE_SPEED = 125;
    

    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        incompatibilityList.Add("Guided");
        incompatibilityList.Add("Homing");
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Active == false) {
            return;
        }
        if(weapIsReturning) {
            WeaponNode.Velocity = CalcReturnVec(WeaponNode.GlobalPosition);
            if(WeaponNode.Item1 is SplitToThree) {
                Weapon w = ((SplitToThree)WeaponNode.Item1).Weaps[0];
                w.Velocity = CalcReturnVec(w.GlobalPosition);
                w = ((SplitToThree)WeaponNode.Item1).Weaps[1];
                w.Velocity = CalcReturnVec(w.GlobalPosition);
            }
            if(WeaponNode.Item2 is SplitToThree) {
                Weapon w = ((SplitToThree)WeaponNode.Item2).Weaps[0];
                w.Velocity = CalcReturnVec(w.GlobalPosition);
                w = ((SplitToThree)WeaponNode.Item2).Weaps[1];
                w.Velocity = CalcReturnVec(w.GlobalPosition);
            }
            if(WeaponNode.CurrentState == Weapon.States.HELD) {
                weapIsReturning = false;
            }
        }
        else if(weapIsReturning == false && WeaponNode.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
    }


    Vector2 CalcReturnVec(Vector2 gPos) {
        return (PlayerNode.GlobalPosition - gPos).Normalized() * RETRIEVE_SPEED;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Switch(true, Active);
    }


}
