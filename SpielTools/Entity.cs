using System.Numerics;

namespace SpielTools;

public abstract class Entity
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Size { get; set; }
    public bool IsGrounded { get; protected set; }

    // Jede Entity entscheidet selbst, wie sie auf die Zeit reagiert
    public virtual void Update(float deltaTime, World world)
    {
        // Einfache Bewegung basierend auf der Geschwindigkeit
        Position += Velocity * deltaTime;
    }
}