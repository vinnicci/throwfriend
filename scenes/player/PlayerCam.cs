using Godot;
using System;

public class PlayerCam : Camera2D
{
    public Node2D ParentNode {get; set;}
    public bool IsShaking {get; private set;}


    public override void _Ready()
    {
        base._Ready();
        IsShaking = false;
    }


    const float LERP_RETURN = 0.03f;
    const float VDIST = 0.5f;


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(ParentNode is Player == false) {
            return;
        }
        float mousePos = (GetGlobalMousePosition().y - GlobalPosition.y) * VDIST;
        Vector2 newOffset = new Vector2();
        newOffset.x = Godot.Mathf.Lerp(Offset.x, 0, LERP_RETURN);
        newOffset.y = Godot.Mathf.Lerp(Offset.y, mousePos, LERP_RETURN);
        newOffset.y = Godot.Mathf.Clamp(newOffset.y, -500, 500);
        Offset = newOffset;
    }


    public async void ShowRestart() {
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        ((Button)GetNode("CanvasLayer/Restart")).Visible = true;
    }


    void OnRestartPressed() {
        ((Button)GetNode("CanvasLayer/Restart")).Visible = false;
        Main mainNode = (Main)GetNode("/root/Main");
        if(IsInstanceValid(mainNode.PlayerSaveFile) == false) {
            GetTree().ReloadCurrentScene();
            return;
        }
        mainNode.LoadGame();
    }


}
