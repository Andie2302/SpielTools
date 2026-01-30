using Raylib_cs;
using SpielTools;

public class KeyboardController : IController
{
    public GameAction GetAction(Player player, World world)
    {
        // 1. WICHTIG: Sprung zuerst prüfen!
        // Wir nutzen IsKeyPressed (feuert nur beim ersten Druck), damit der Spieler
        // im nächsten Frame sofort wieder steuern kann (Air Control).
        if (Raylib.IsKeyPressed(KeyboardKey.Space)) 
        {
            return GameAction.Jump; 
        }

        // 2. Bewegung prüfen (wird nur erreicht, wenn NICHT gerade gesprungen wurde)
        if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) 
        {
            return GameAction.MoveRight;
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) 
        {
            return GameAction.MoveLeft;
        }

        return GameAction.Idle;
    }
}