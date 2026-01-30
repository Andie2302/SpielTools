using System.Numerics;

namespace SpielTools;

public class Obstacle : Entity
{
    // Wir übergeben direkt Position und Größe im Konstruktor
    public Obstacle(float x, float y, float width, float height)
    {
        Position = new Vector2(x, y);
        Size = new Vector2(width, height);
    }

    public override void Update(float deltaTime, World world)
    {
        // 1. Das Hindernis bewegt sich (noch) nicht von selbst.
        // Falls du später bewegliche Gegner willst, kommt hier: Position.X -= 50 * deltaTime;
        
        // 2. WICHTIG: Bodenhaftung!
        // Wir setzen das Hindernis immer exakt auf die Höhe des Bodens an seiner X-Position.
        var groundY = world.GetGroundY(Position.X);
        
        // Da Position.Y bei uns die "Füße" (unten) sind, setzen wir es gleich groundY.
        Position = Position with { Y = groundY };
    }
}