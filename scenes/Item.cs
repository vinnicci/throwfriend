using Godot;
using System;

public class Item : Node2D
{
    public override void _Ready()
    {
    }

    
    /// <summary>
    /// Apply effects during intialization
    /// </summary>
    public virtual void InitEffect() {}


    /// <summary>
    /// Use this to apply effect anytime
    /// </summary>
    public virtual void ApplyEffect() {}


//  public override void _Process(float delta)
//  {
//  }


}
