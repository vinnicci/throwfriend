using Godot;
using System;

public class ParaPrin : Enemy
{
    public EnemyWeapon Weapon2 {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        //Weapon1 = (ParaPrinWeap1)GetNode("Sprite/Skirt/Torso/Skeleton2D/Spine/Spine2/Spine3/EnemyWeapon");
        //Weapon1.ParentNode = this;
        //Weapon2 = 
    }


    private float impulseRot;


    // public void MeleeAttack() {
    //     Weapon1.LookAt(LevelNode.GetPlayerPos());
    //     impulseRot = Weapon1.GlobalRotation;
    //     float dotProd =
    //     new Vector2(0,1).Dot(new Vector2(1,0).Rotated(impulseRot));
    //     if(dotProd <= -0.5) {
    //         //anim.Play("melee_attack_up");
    //         anim.Play("melee_attack_up");
    //         Weapon1.MeleeAttackDir(true);
    //     }
    //     else {
    //         //anim.Play("melee_attack_down");
    //         anim.Play("melee_attack_down");
    //         Weapon1.MeleeAttackDir(false);
    //     }
    // }


    public override void AdjustSprites()
    {
        base.AdjustSprites();
    }


    const int FORWARD_IMPULSE = 250;


    public virtual void AttackImpulse() {
        ApplyCentralImpulse(new Vector2(1,0).Rotated(impulseRot) * FORWARD_IMPULSE);
    }


    public override void FinishAction(string actionName)
    {
        base.FinishAction(actionName);
    }

    

}
