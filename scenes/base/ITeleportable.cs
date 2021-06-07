using Godot;

public interface ITeleportable
{
    void Teleport(Level level, Vector2 globalPos);
    void FreeSprite(Godot.Object teleSprite);
}
