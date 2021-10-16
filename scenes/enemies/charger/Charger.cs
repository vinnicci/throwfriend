using Godot;
using System;

public class Charger : BaseCharger
{
    public override void OnEnemyBodyEntered(Godot.Object body) {
        base.OnEnemyBodyEntered(body);
    }


    public override void Charge() {
        base.Charge();
    }


    public new void PlaySoundEffect(String soundName) {
        base.PlaySoundEffect(soundName);
    }


}
