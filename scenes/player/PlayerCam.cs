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


    const float LERP_RETURN = 0.04f;
    const float VDIST = 0.5f;
    const float HDIST = 0.12f;


    void ShiftToCenter() {
        float mousePosY = (GetGlobalMousePosition().y - GlobalPosition.y) * VDIST;
        float mousePosX = (GetGlobalMousePosition().x - GlobalPosition.x) * HDIST;
        Vector2 newOffset = new Vector2();
        newOffset.x = Mathf.Lerp(Offset.x, mousePosX, LERP_RETURN);
        newOffset.x = Mathf.Clamp(newOffset.x, -500, 500);
        newOffset.y = Mathf.Lerp(Offset.y, mousePosY, LERP_RETURN);
        newOffset.y = Mathf.Clamp(newOffset.y, -500, 500);
        Offset = newOffset;
    }


    Vector2 amp;
    bool rand;
    int currentPriority;


    public void ShakeCamera(Vector2 amp, float frequency, float duration, int priority = 0, bool randomized = true) {
        if(ParentNode is Player == false || priority < currentPriority) {
            return;
        }
        this.amp = amp;
        rand = randomized;
        IsShaking = true;
        currentPriority = priority;
        freqTimer.WaitTime = frequency;
        durationTimer.WaitTime = duration;
        freqTimer.Start();
        durationTimer.Start();
        Shake();
    }


    void Shake() {
        Vector2 dirShake = Vector2.Zero;
        if(rand) {
            dirShake = new Vector2((float)GD.RandRange(-amp.x, amp.x) + Offset.x, (float)GD.RandRange(-amp.y, amp.y) + Offset.y);
        }
        else {
            dirShake = new Vector2(amp.x + Offset.x, amp.y + Offset.y);
        }
        shakeTween.InterpolateProperty(this, "offset", Offset, dirShake, freqTimer.WaitTime,
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
