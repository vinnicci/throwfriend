using Godot;
using System;

public class EliteRangedShielded : AllRounder
{
    public override void ShootBack() {
        base.ShootBack();
    }


    public override void ThrowBlob()
    {
        base.ThrowBlob();
    }


    public override void Recoil() {
        base.Recoil();
    }


    public void SuperScatter() {
        ((EliteBlasterAndShield)WeaponNode).SuperScatter();
    }


    public void TripleShot() {
        ((EliteBlasterAndShield)WeaponNode).TripleShot();
    }


}
