using Godot;
public interface IHealthModifiable
{
    int Health {get; set;}
    void Hit(Vector2 knockback, int damage);
}
