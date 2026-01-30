namespace SpielTools;

public class World
{
    private readonly List<GroundPoint> _groundPoints = [];
    public IReadOnlyList<GroundPoint> Points => _groundPoints; // <-- Das hier erlaubt Lesen, aber kein Schreiben

    public List<Entity> Entities { get; } = [];

    /// <summary>
    /// Fügt einen neuen Eckpunkt zur Landschaft hinzu.
    /// </summary>
    public void AddGroundPoint(float x, float y)
    {
        _groundPoints.Add(new GroundPoint(x, y));
        _groundPoints.Sort((a, b) => a.X.CompareTo(b.X));
    }

    /// <summary>
    /// Berechnet die Bodenhöhe an einer beliebigen X-Position.
    /// Gibt float.MaxValue zurück, wenn dort ein Loch ist.
    /// </summary>
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

    public void Update(float deltaTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(deltaTime, this);
        }
    }

    // NEU: Liste für Hindernisse
    private readonly List<Obstacle> _obstacles = new();
    public IReadOnlyList<Obstacle> Obstacles => _obstacles;

    // NEU: Methode zum Hinzufügen
    public void AddObstacle(Obstacle obs)
    {
        _obstacles.Add(obs);
        Entities.Add(obs); // Auch zur Hauptliste, damit Update() aufgerufen wird
    }
    
}