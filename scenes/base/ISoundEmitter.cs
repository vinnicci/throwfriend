using Godot;
using System;

public interface ISoundEmitter
{
    Node2D SoundsNode {get; set;}
    Godot.Collections.Dictionary<String, AudioStreamPlayer2D> SoundsDict {get; set;}
    void PlaySoundEffect(string soundName);
}