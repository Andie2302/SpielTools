namespace SpielTools;

/// <summary>
/// Ein einfacher Punkt in der Landschaft.
/// </summary>
public record struct GroundPoint(float X, float Y);

public class World
{
    // Die Liste muss immer nach X sortiert sein, damit unsere Logik funktioniert!
    private readonly List<GroundPoint> _groundPoints = new();
    
    // Die Entities, die wir vorhin besprochen haben
    public List<Entity> Entities { get; } = new();

    /// <summary>
    /// Fügt einen neuen Eckpunkt zur Landschaft hinzu.
    /// </summary>
    public void AddGroundPoint(float x, float y)
    {
        _groundPoints.Add(new GroundPoint(x, y));
        // Wir sortieren sicherheitshalber nach jedem Hinzufügen, 
        // damit die Punkte 1, 2, 3... in der richtigen Reihenfolge liegen.
        _groundPoints.Sort((a, b) => a.X.CompareTo(b.X));
    }

    /// <summary>
    /// Berechnet die Bodenhöhe an einer beliebigen X-Position.
    /// Gibt float.MaxValue zurück, wenn dort ein Loch ist.
    /// </summary>
    public float GetGroundY(float playerX)
    {
        // Fall 1: Keine Welt definiert -> Unendlicher Fall
        if (_groundPoints.Count < 2) return float.MaxValue;

        // Fall 2: Spieler ist links vom allerersten Punkt -> Fall ins Loch
        if (playerX < _groundPoints[0].X) return float.MaxValue;

        // Fall 3: Spieler ist rechts vom allerletzten Punkt -> Fall ins Loch
        if (playerX > _groundPoints[^1].X) return float.MaxValue;

        // Haupt-Logik: Wir suchen das Segment, in dem der Spieler steht
        for (int i = 0; i < _groundPoints.Count - 1; i++)
        {
            var p1 = _groundPoints[i];
            var p2 = _groundPoints[i + 1];

            // Ist der Spieler zwischen p1 und p2?
            if (playerX >= p1.X && playerX <= p2.X)
            {
                // Hier kommt die Mathematik:
                // 1. Wie lang ist die Strecke zwischen den Punkten?
                float segmentWidth = p2.X - p1.X;
                
                // 2. Wie weit ist der Spieler von p1 entfernt?
                float distFromP1 = playerX - p1.X;

                // 3. Prozentualer Fortschritt (0.0 bis 1.0)
                // Wenn segmentWidth fast 0 ist (senkrechte Wand), vermeiden wir Division durch Null
                if (segmentWidth < 0.001f) return p1.Y; 
                
                float t = distFromP1 / segmentWidth;

                // 4. Lineare Interpolation für die Höhe
                // Formel: StartHöhe + (Unterschied * Fortschritt)
                return p1.Y + (p2.Y - p1.Y) * t;
            }
        }

        // Sollte theoretisch nie erreicht werden, wenn die oberen Checks greifen
        return float.MaxValue;
    }

    public void Update(float deltaTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(deltaTime, this);
        }
    }
}