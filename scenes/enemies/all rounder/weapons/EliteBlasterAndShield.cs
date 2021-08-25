using Godot;
using System;

public class EliteBlasterAndShield : AllRounderWeapon
{
    public void SuperScatter() {
        Anim.Play("super_scatter");
    }


    public void TripleShot() {
        Anim.Play("triple_shot");
    }


    public override void SpawnInstance(string packedSceneKey, int count = 1) {
        if(packedSceneKey == "proj") {
            if(count == 1) {
                SpawnProj();
            }
            else if(count == 2) {
                SpawnProj2();
            }
        }
        else {
            base.SpawnInstance(packedSceneKey, count);
        }
    }


}
