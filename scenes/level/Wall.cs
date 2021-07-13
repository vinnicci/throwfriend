using Godot;
using System;

public class Wall : TileMap, IHealthModifiable
{
    [Export] protected int health = 10;
    [Export] protected bool destructible = false;
    
    public int Health {get; set;}
    public Timer HitCooldown {
        get {
            GD.PrintErr("No hitcooldown timer for Level nodes.");
            return null;
        } set {
            GD.PrintErr("No hitcooldown timer for Level nodes.");
        }
    }
    public AnimationPlayer DamageAnim {get; set;}


    public override void _Ready()
    {
        base._Ready();
        Health = health;
    }


    public bool Hit(Vector2 knockback, int damage) {
    if(destructible == false) {
        return false;
    }
    //add destroyed wall effect
    return true;
    }

    
}
