using System.Numerics;
using SpielTools;

// 1. Welt mit Rampe bauen
var world = new World();
world.AddGroundPoint(0, 100);   // Start unten
world.AddGroundPoint(20, 100);  // Beginn Rampe
world.AddGroundPoint(40, 50);   // Ende Rampe (oben)
world.AddGroundPoint(80, 50);   // Plateau
var player = new Player();
player.Position = new Vector2(10, 50); 
player.Controller = new AutoWalker(); // <-- WICHTIG: Hier bekommt er das Gehirn
world.Entities.Add(player);

Console.WriteLine("--- TEST: Rampe hochlaufen ---");

// Engine-Setup (optional, du kannst auch direkt world.Update rufen)
// const float deltaTime = 0.016f; 

// Wir simulieren 200 Frames
for (var i = 1; i <= 200; i++)
{
    world.Update(0.016f);
    
    // Ausgabe alle 20 Frames
    if (i % 20 == 0)
    {
        string ort = "LUFT";
        if (player.Position.Y == 100) ort = "UNTEN";
        else if (player.Position.Y < 100 && player.Position.Y > 50) ort = "RAMPE";
        else if (player.Position.Y == 50) ort = "OBEN";
        
        Console.WriteLine($"T={i*0.016f:F2}s | X={player.Position.X:F1} Y={player.Position.Y:F1} -> {ort}");
    }
}

// 2. Ein dummer Bot, der immer nach rechts läuft
class AutoWalker : IController
{
    public GameAction GetAction(Player p, World w)
    {
        // Einfach immer nach rechts laufen!
        return GameAction.MoveRight;
    }
}

// 3. Spieler mit dem Bot verknüpfen