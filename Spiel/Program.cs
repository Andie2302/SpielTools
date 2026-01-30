using System.Numerics;
using SpielTools;

// --- KONFIGURATION ---
bool useGraphics = true;   // TRUE = Raylib Fenster, FALSE = Nur Konsolentext
bool useAI = false;         // TRUE = AutoWalker, FALSE = Tastatur

// 1. Welt bauen
var world = new World();
world.AddGroundPoint(0, 100);
world.AddGroundPoint(100, 100); // Ebene
world.AddGroundPoint(200, 50);  // Rampe hoch
world.AddGroundPoint(300, 50);  // Plateau
world.AddGroundPoint(400, 200); // Abgrund / Loch
world.AddGroundPoint(600, 200); 

// 2. Spieler und Controller wählen
var player = new Player();
player.Position = new Vector2(50, 50); // Start in der Luft

if (useAI)
{
    // Dein alter AutoWalker Code (kannst du auch in eigene Datei auslagern)
    player.Controller = new AutoWalker(); 
}
else
{
    // Falls Grafik an ist, nehmen wir Tastatur. Sonst macht Tastatur keinen Sinn.
    if (useGraphics) player.Controller = new KeyboardController();
}

world.Entities.Add(player);


// 3. STARTEN
if (useGraphics)
{
    // Grafik-Modus: Das Fenster übernimmt die Schleife
    var renderer = new GameRenderer();
    renderer.RunWindow(world, player);
}
else
{
    // Headless-Modus (z.B. für KI-Training auf Server)
    Console.WriteLine("Starte Simulation ohne Grafik...");
    for (int i = 0; i < 500; i++)
    {
        world.Update(0.016f);
        // ... dein alter Logging Code ...
        if (i % 20 == 0) Console.WriteLine($"T={i}: {player.Position}");
    }
}

// Hilfsklasse für KI (falls nicht ausgelagert)
class AutoWalker : IController
{
    public GameAction GetAction(Player p, World w) => GameAction.MoveRight;
}