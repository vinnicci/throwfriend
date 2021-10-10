using Godot;
using System;

public class PlayerCam : Camera2D
{
    public Node2D ParentNode {get; set;}
    public bool IsShaking {get; private set;}

    Timer freqTimer;
    Timer durationTimer;
    Tween shakeTween;


    public override void _Ready()
    {
        base._Ready();
        IsShaking = false;
        freqTimer = (Timer)GetNode("Frequency");
        durationTimer = (Timer)GetNode("Duration");
        shakeTween = (Tween)GetNode("ShakeTween");
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(ParentNode is Player == false) {
            return;
        }
        ShiftToCenter();
    }


    const float LERP_RETURN = 0.03f;
    const float VDIST = 0.5f;


    void ShiftToCenter() {
        float mousePos = (GetGlobalMousePosition().y - GlobalPosition.y) * VDIST;
        Vector2 newOffset = new Vector2();
        newOffset.x = Mathf.Lerp(Offset.x, 0, LERP_RETURN);
        newOffset.y = Mathf.Lerp(Offset.y, mousePos, LERP_RETURN);
        newOffset.y = Mathf.Clamp(newOffset.y, -500, 500);
        Offset = newOffset;
    }


    float amp;
    int currentPriority;


    public void ShakeCamera(float amp, float frequency, float duration, int priority = 0) {
        if(ParentNode is Player == false || priority < currentPriority) {
            return;
        }
        this.amp = amp;
        IsShaking = true;
        currentPriority = priority;
        freqTimer.WaitTime = frequency;
        durationTimer.WaitTime = duration;
        freqTimer.Start();
        durationTimer.Start();
        Shake();
    }


    void Shake() {
        float randX = (float)GD.RandRange(-amp, amp);
        float randY = (float)GD.RandRange(-amp, amp);
        Vector2 ampVector = new Vector2(randX, randY + Offset.y);
        shakeTween.InterpolateProperty(this, "offset", Offset, ampVector, freqTimer.WaitTime,
        Tween.TransitionType.Sine, Tween.EaseType.InOut);
        shakeTween.Start();
    }


    void OnFrequencyTimeout() {
        Shake();
    }


    void OnDurationTimeout() {
        StopShake();
    }


    void StopShake() {
        currentPriority = 0;
        IsShaking = false;
        freqTimer.Stop();
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
