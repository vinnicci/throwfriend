using Godot;
using System;

public class WorldMarker : Position2D
{
    [Export] public Godot.Collections.Array<String> levelFiles =
    new Godot.Collections.Array<String>();
    [Export] public String levelName = "";
}
