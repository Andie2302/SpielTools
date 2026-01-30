using Raylib_cs;

namespace SpielTools;

public class KeyboardController : IController
{
    public GameAction GetAction(Player player, World world)
    {
        // Raylib fragt direkt die Tastatur ab
        if (Raylib.IsKeyDown(KeyboardKey.Right)) return GameAction.MoveRight;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) return GameAction.MoveLeft;
        if (Raylib.IsKeyPressed(KeyboardKey.Space)) return GameAction.Jump; // KeyPressed = Nur einmal pro Druck

        return GameAction.Idle;
    }
}