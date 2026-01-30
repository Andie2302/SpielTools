namespace SpielTools;

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