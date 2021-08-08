using Godot;

public interface ISpawnable
{
    ///<summary>
    /// For the destination, provide local position. Used by projectiles to modify range.
    ///</summary>
    void Spawn(Level lvl, Vector2 globalPos, Vector2 destination, float globalRot = 0);
}