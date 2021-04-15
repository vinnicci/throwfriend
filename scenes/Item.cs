using Godot;
using System;
using System.Collections.Generic;

public class Item : Node2D
{
    protected List<String> incompatibilityList = new List<string>();


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


    /// <summary>
    /// Remove applied effects upon removal of item
    /// </summary>
    public virtual void RemoveEffect() {}


}
