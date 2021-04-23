using Godot;
using System;

public class PlayerCam : Camera2D
{
    public Node2D ParentNode {get; set;}
    public bool IsShaking {get; private set;}
    private const float VDIST = 0.5f;


    public override void _Ready()
    {
        base._Ready();
        IsShaking = false;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(ParentNode is Player == false) {
            return;
        }
        float mousePos = (GetGlobalMousePosition().y - GlobalPosition.y) * VDIST;
        Vector2 newOffset = new Vector2();
        newOffset.x = Godot.Mathf.Lerp(Offset.x, 0, 0.02f);
        newOffset.y = Godot.Mathf.Lerp(Offset.y, mousePos, 0.02f);
        newOffset.y = Godot.Mathf.Clamp(newOffset.y, -500, 500);
        Offset = newOffset;
    }


}
