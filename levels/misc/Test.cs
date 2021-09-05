using Godot;
using System;

public class Test : Level
{
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(Input.IsActionJustPressed("ui_page_up") &&
        PlayerNode.WeaponNode.CurrentState != Weapon.States.HELD) {
            PlayerNode.WeaponNode.Teleport(this, GetGlobalMousePosition());
        }
    }

}
