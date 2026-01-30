using System.Numerics;

namespace SpielTools;

public class World
{
    private readonly List<GroundPoint> _groundPoints = [];
    public IReadOnlyList<GroundPoint> Points => _groundPoints;

    public List<Entity> Entities { get; } = [];

    // --- NEU: Spielzustand ---
    public float Score { get; private set; }
    public bool IsGameOver { get; set; }

    // --- NEU: Hindernisse ---
    private readonly List<Obstacle> _obstacles = new();
    public IReadOnlyList<Obstacle> Obstacles => _obstacles;

    public void AddGroundPoint(float x, float y)
    {
        _groundPoints.Add(new GroundPoint(x, y));
        _groundPoints.Sort((a, b) => a.X.CompareTo(b.X));
    }

    public void AddObstacle(Obstacle obs)
    {
        _obstacles.Add(obs);
        Entities.Add(obs);
    }

    public float GetGroundY(float playerX)
    {
        if (_groundPoints.Count < 2 || playerX < _groundPoints[0].X || playerX > _groundPoints[^1].X) return float.MaxValue;

        for (var i = 0; i < _groundPoints.Count - 1; i++)
        {
            var p1 = _groundPoints[i];
            var p2 = _groundPoints[i + 1];

            if (!(playerX >= p1.X) || !(playerX <= p2.X)) continue;
            var segmentWidth = p2.X - p1.X;
            var distFromP1 = playerX - p1.X;

            if (segmentWidth < 0.001f) return p1.Y; 
                
            var t = distFromP1 / segmentWidth;
            return p1.Y + (p2.Y - p1.Y) * t;
        }
        return float.MaxValue;
    }

    // --- NEU: Reset-Logik ---
    public void Reset()
    {
        Score = 0;
        IsGameOver = false;

        // Setze alle Spieler zurück
        foreach (var entity in Entities)
        {
            if (entity is Player p)
            {
                p.Position = new Vector2(0, 0); // Startpunkt
                p.Velocity = Vector2.Zero;
                // Wichtig: Wir müssen dem Spieler sagen, dass er nicht mehr am Boden klebt
                // Da wir IsGrounded in Entity nicht direkt setzen können (protected), 
                // wird das beim nächsten Update automatisch korrigiert.
            }
        }
    }

    public void Update(float deltaTime)
    {
        if (IsGameOver) return; // Wenn tot, keine Updates mehr!

        // Score: +10 Punkte pro Sekunde überleben
        Score += 10 * deltaTime;

        foreach (var entity in Entities)
        {
            entity.Update(deltaTime, this);
        }
    }
}