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


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(ParentNode is Player == false) {
            return;
        }
        ShiftV();
        //ShiftZoom();
    }


    const float LERP_RETURN = 0.03f;
    const float VDIST = 0.5f;


    void ShiftV() {
        float mousePos = (GetGlobalMousePosition().y - GlobalPosition.y) * VDIST;
        Vector2 newOffset = new Vector2();
        newOffset.x = Mathf.Lerp(Offset.x, 0, LERP_RETURN);
        newOffset.y = Mathf.Lerp(Offset.y, mousePos, LERP_RETURN);
        newOffset.y = Mathf.Clamp(newOffset.y, -500, 500);
        Offset = newOffset;
    }


    // const float DEFAULT_ZOOM = 0.5f;
    // const int DEF_ZOOM_DIST_DENOM = 62500;
    // const float ZOOM_SPEED = 0.03f;
    // Player player;


    // void ShiftZoom() {
    //     if(ParentNode is Player == false && Zoom != new Vector2(DEFAULT_ZOOM, DEFAULT_ZOOM)) {
    //         Zoom = new Vector2(DEFAULT_ZOOM, DEFAULT_ZOOM);
    //         return;
    //     }
    //     if(player is Player == false) {
    //         player = (Player)ParentNode;
    //     }
    //     int dist = (int)player.GlobalPosition.DistanceSquaredTo(player.WeaponNode.GlobalPosition);
    //     float zoomF = (0.5f*dist)/DEF_ZOOM_DIST_DENOM;
    //     Vector2 newZoom = new Vector2();
    //     newZoom.x = Mathf.Lerp(Zoom.x, zoomF, ZOOM_SPEED);
    //     newZoom.x = Mathf.Clamp(newZoom.x, DEFAULT_ZOOM, 1);
    //     newZoom.y = Mathf.Lerp(Zoom.y, zoomF, ZOOM_SPEED);
    //     newZoom.y = Mathf.Clamp(newZoom.y, DEFAULT_ZOOM, 1);
    //     Zoom = newZoom;
    // }


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
