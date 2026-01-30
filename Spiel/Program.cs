using System.Numerics;
using SpielTools;

// 1. Welt bauen (Wie vorher)
var world = new World();
world.AddGroundPoint(0, 100);
world.AddGroundPoint(50, 100);

// 2. Spieler erstellen
var player = new Player();
player.Position = new Vector2(10, 0); // Startet bei X=10, Y=0 (Luft)
world.Entities.Add(player);

Console.WriteLine($"START: Spieler Y={player.Position.Y} (In der Luft)");

// 3. Simulationsschleife (Wir simulieren 60 Frames)
// Wir nutzen einen festen Takt von 0.016s (ca 60 FPS)
float deltaTime = 0.016f;

for (int i = 1; i <= 100; i++)
{
    // Engine-Schritt
    world.Update(deltaTime);
    
    // Nur alle 10 Frames eine Ausgabe machen, damit die Konsole lesbar bleibt
    if (i % 10 == 0)
    {
        string status = player.IsGrounded ? "AM BODEN" : "FÄLLT";
        Console.WriteLine($"Frame {i}: Y={player.Position.Y:F2} Speed={player.Velocity.Y:F2} -> {status}");
    }
}