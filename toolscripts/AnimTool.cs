using Godot;
using System;

[Tool]
public class AnimTool : AnimationPlayer
{
    [Export] String animName;

    enum UpdateModes {
        CONTINUOUS, DISCRETE, TRIGGER, CAPTURE
    }
    UpdateModes updateMode = UpdateModes.CONTINUOUS;
    [Export] UpdateModes UpdateMode {
        get {
            return updateMode;
        }
        set {
            updateMode = value;
            ChangeUpdateMode(updateMode);
        }
    }

    enum InterpolationModes {
        NEAREST, LINEAR, CUBIC
    }
    InterpolationModes interpolationMode = InterpolationModes.LINEAR;
    [Export] InterpolationModes InterpolationMode {
        get {
            return interpolationMode;
        }
        set {
            interpolationMode = value;
            ChangeInterpolationMode(interpolationMode);
        }
    }


    void ChangeUpdateMode(UpdateModes newUpdateMode) {
        if(HasAnimation(animName) == false) {
            return;
        }
        for(int i = 0; i <= GetAnimation(animName).GetTrackCount() - 1; i++) {
            GetAnimation(animName).ValueTrackSetUpdateMode(i, (Animation.UpdateMode)newUpdateMode);
        }
    }


    void ChangeInterpolationMode(InterpolationModes newInterpolationMode) {
        if(HasAnimation(animName) == false) {
            return;
        }
        for(int i = 0; i <= GetAnimation(animName).GetTrackCount() - 1; i++) {
            GetAnimation(animName).TrackSetInterpolationType(i, (Animation.InterpolationType)newInterpolationMode);
        }
    }


}
