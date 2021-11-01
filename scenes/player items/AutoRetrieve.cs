using Godot;
using System;

public class AutoRetrieve : PlayerItem
{
    public override void _Ready()
    {
        base._Ready();
        incompatibilityList.Add("AutoRetrieve");
        //incompatibilityList.Add("Guided");
        //incompatibilityList.Add("Homing");
        //incompatibilityList.Add("Boost");
    }


    bool weapIsReturning = false;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Active == false) {
            return;
        }
        if(weapIsReturning) {
            WeaponNode.Velocity = CalcReturnVec(WeaponNode.GlobalPosition);
            if(WeaponNode.Item1 is SplitToThree) {
                ApplyEffectToClones((SplitToThree)WeaponNode.Item1);
            }
            if(WeaponNode.Item2 is SplitToThree) {
                ApplyEffectToClones((SplitToThree)WeaponNode.Item2);
            }
            if(WeaponNode.CurrentState == Weapon.States.HELD) {
                weapIsReturning = false;
            }
        }
        else if(weapIsReturning == false && WeaponNode.CurrentState == Weapon.States.INACTIVE) {
            weapIsReturning = true;
        }
    }


    void ApplyEffectToClones(SplitToThree item) {
        Weapon w = item.Weaps[0];
        w.Velocity = CalcReturnVec(w.GlobalPosition);
        w = item.Weaps[1];
        w.Velocity = CalcReturnVec(w.GlobalPosition);
    }


    const int NEAR_DIST = 10000;
    const int FAR_RETRIEVE_SPEED = 250;
    const int NEAR_RETRIEVE_SPEED = 125;


    Vector2 CalcReturnVec(Vector2 gPos) {
        if(IsInstanceValid(PlayerNode) == false) {
            return Vector2.Zero;
        }
        else if(gPos.DistanceSquaredTo(PlayerNode.GlobalPosition) <= NEAR_DIST) {
            return (PlayerNode.GlobalPosition - gPos).Normalized() * NEAR_RETRIEVE_SPEED;
        }
        return (PlayerNode.GlobalPosition - gPos).Normalized() * FAR_RETRIEVE_SPEED;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Switch(true, Active == false);
    }


}
