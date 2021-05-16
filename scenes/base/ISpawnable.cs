using Godot;

public interface ISpawnable
{
    void Spawn(Level lvl, Vector2 globalPos, float globalRot = 0);
}