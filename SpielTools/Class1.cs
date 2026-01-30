using System.Numerics;

namespace SpielTools;


public class GameEngine
{
    public float PhysicsDeltaTime { get; set; } = 0.01f;
    private readonly World _world;

    public GameEngine(World world)
    {
        _world = world;
    }

    public void DoStep()
    {
        // Die gesamte Magie passiert hier
        _world.Update(PhysicsDeltaTime);
    }
}



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


public class World
{
    public List<Entity> Entities { get; } = new();
    // Hier kommen später die GroundPoints für die Schrägen rein
    
    public void Update(float deltaTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(deltaTime, this);
        }
    }
}