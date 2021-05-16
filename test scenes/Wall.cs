using Godot;
using System;

public class Wall : TileMap, IHealthModifiable
{
    [Export] protected int health = 10;
    [Export] protected bool destructible = false;

    public int Health {get; set;}


    public override void _Ready()
    {
        base._Ready();
        Health = health;
    }


    public void Hit(Vector2 knockback, int damage) {
    if(destructible == false) {
        return;
    }
    //add destroyed wall effect
    }

    
}
