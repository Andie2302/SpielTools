using System.Numerics;
using Raylib_cs;

namespace SpielTools;

public class GameRenderer
{
    public void RunWindow(World world, Player player)
    {
        Raylib.InitWindow(1000, 600, "Jump & Run");
        Raylib.SetTargetFPS(60);
        Camera2D camera = new() { Zoom = 1.5f, Offset = new Vector2(500, 400) };

        while (!Raylib.WindowShouldClose())
        {
            
            // --- NEU: RESET ---
            // Wenn das Spiel vorbei ist UND 'R' gedrückt wird -> Alles auf Anfang
            if (world.IsGameOver && Raylib.IsKeyPressed(KeyboardKey.R))
            {
                world.Reset();
            }
            
            // Update nur, wenn Spiel läuft (oder wir lassen World.Update das regeln)
            world.Update(Raylib.GetFrameTime());

            // Zeichnen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            
            // Kamera verfolgt Spieler (auch im Tod, damit wir sehen wo wir liegen)
            camera.Target = player.Position; 
            Raylib.BeginMode2D(camera);

            // ... Boden & Hindernisse zeichnen (wie gehabt) ...
            var points = world.Points;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Raylib.DrawLineEx(new Vector2(points[i].X, points[i].Y), new Vector2(points[i+1].X, points[i+1].Y), 4f, Color.DarkGreen);
            }

            // Spieler
            Raylib.DrawRectangle((int)player.Position.X, (int)player.Position.Y - 20, 20, 20, Color.Red);

            Raylib.EndMode2D();

            // UI Overlay
            if (world.IsGameOver)
            {
                Raylib.DrawText("GAME OVER", 350, 200, 50, Color.Red);
                Raylib.DrawText("Drücke 'R' für Neustart", 370, 260, 20, Color.DarkGray);
            }
            else
            {
                Raylib.DrawText($"Score: {(int)world.Score}", 10, 10, 20, Color.Black);
            }

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}