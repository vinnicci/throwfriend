using Godot;

public interface ISpawnable
{
    ///<summary>
    /// For the destination, provide local position. Used by projectiles to modify range.
    /// if target is valid, projectile will home towards it
    ///</summary>
    void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0, bool homeToPlayer = false);
}