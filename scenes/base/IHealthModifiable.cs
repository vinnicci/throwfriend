using Godot;

public interface IHealthModifiable
{
    Timer HitCooldown {get; set;}
    AnimationPlayer DamageAnim {get; set;}
    int Health {get; set;}
    bool Hit(Vector2 knockback, int damage);
}
