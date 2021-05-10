using Godot;
using System;

public class ExItemExplosion : Explosion
{
    public override void ApplyDamage(Entity en)
    {
        if(en is Player) {
            return;
        }
        else {
            base.ApplyDamage(en);
        }
    }


}
