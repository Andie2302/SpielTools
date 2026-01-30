using System.Numerics;

namespace SpielTools;

public class World
{
    private readonly List<GroundPoint> _groundPoints = [];
    public IReadOnlyList<GroundPoint> Points => _groundPoints;

    public List<Entity> Entities { get; } = [];

    // --- NEU: Spielzustand & Score ---
    public float Score { get; private set; }
    public bool IsGameOver { get; set; }

    // Hindernisse
    private readonly List<Obstacle> _obstacles = new();
    public IReadOnlyList<Obstacle> Obstacles => _obstacles;

 
    public void Update(float deltaTime)
    {
        // WICHTIG: Wenn Game Over ist, frieren wir die Welt ein!
        // Deshalb konntest du dich bei 502,5 nicht mehr bewegen.
        if (IsGameOver) return; 

        Score += 10 * deltaTime; // Punkte fürs Überleben

        foreach (var entity in Entities)
        {
            entity.Update(deltaTime, this);
        }
    }

    public void AddGroundPoint(float x, float y)
    {
        _groundPoints.Add(new GroundPoint(x, y));
        _groundPoints.Sort((a, b) => a.X.CompareTo(b.X));
    }

    public float GetGroundY(float playerX)
    {
        // Deine bestehende Logik für GetGroundY ...
        if (_groundPoints.Count < 2 || playerX < _groundPoints[0].X || playerX > _groundPoints[^1].X) return float.MaxValue;

        for (var i = 0; i < _groundPoints.Count - 1; i++)
        {
            var p1 = _groundPoints[i];
            var p2 = _groundPoints[i + 1];
            if (playerX >= p1.X && playerX <= p2.X)
            {
                var t = (playerX - p1.X) / (p2.X - p1.X);
                return p1.Y + (p2.Y - p1.Y) * t;
            }
        }
        return float.MaxValue;
    }

    public void AddObstacle(Obstacle obs)
    {
        _obstacles.Add(obs);
        Entities.Add(obs);
    }
    
    // In SpielTools/World.cs

    public void Reset()
    {
        Score = 0;
        IsGameOver = false;

        // Suche den Spieler und setze ihn zurück
        foreach (var entity in Entities)
        {
            if (entity is Player p)
            {
                p.Position = new System.Numerics.Vector2(0, 0); // Startpunkt
                p.Velocity = System.Numerics.Vector2.Zero;      // Stop!
                p.IsGrounded = false; // Er startet in der Luft
            }
        }
        Console.WriteLine("Spiel wurde zurückgesetzt!");
    }
    
}