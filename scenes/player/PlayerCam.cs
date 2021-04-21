using Godot;
using System;

public class PlayerCam : Camera2D
{
    public Node2D ParentNode {get; set;}
    public bool IsShaking {get; private set;}

    private const float VDIST = 0.5f;


    public override void _Ready()
    {
    }


    public override void _PhysicsProcess(float delta)
    {
        if(ParentNode is Player == false || IsShaking == true) {
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
