using Godot;
using System;

public abstract class Wall : TileMap, IHealthModifiable, ILevelObject
{
    [Export] protected int health = 10;
    [Export] protected bool destructible = false;

    AnimationPlayer anim;
    
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
    public string SwitchSignal {get; set;}


    public override void _Ready()
    {
        base._Ready();
        Health = health;
        anim = (AnimationPlayer)GetNode("Anim");
        SwitchSignal = nameof(Switched);
    }


    public bool Hit(Vector2 knockback, int damage) {
        if(destructible == false) {
            return false;
        }
        //add destroyed wall effect, explosive effect
        EmitSignal(nameof(Switched));
        return true;
    }


    //secret walls
    public void FadeOut() {
        EmitSignal(nameof(Switched));
        anim.Play("fade_out");
    }


    [Signal] public delegate void Switched();


    public void Switch() {
        QueueFree();
    }


}
