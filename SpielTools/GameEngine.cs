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