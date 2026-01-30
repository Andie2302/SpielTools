namespace SpielTools;


public class GameEngine(World world)
{
    public float PhysicsDeltaTime { get; set; } = 0.01f;

    public void DoStep()
    {
        world.Update(PhysicsDeltaTime);
    }
}