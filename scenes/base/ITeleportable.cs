using Godot;

public interface ITeleportable
{
    AnimationPlayer TeleportAnim {get; set;}
    void Teleport(Level level, Vector2 globalPos);
    void FreeSprite(Godot.Object teleSprite);
}
