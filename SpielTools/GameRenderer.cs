using System.Numerics;
using Raylib_cs;

namespace SpielTools;

public class GameRenderer
{
    public static void RunWindow(World world, Player player)
    {
        Raylib.InitWindow(1000, 600, "Jump & Run - Test");
        Raylib.SetTargetFPS(60);

        // Kamera-Setup (damit Y=0 unten ist und Y+ nach oben geht, oder wir zoomen rein)
        Camera2D camera = new() { Zoom = 1.5f, Offset = new Vector2(500, 400) };

        while (!Raylib.WindowShouldClose())
        {
            // 1. Logik Update (Nutze die Frame-Zeit von Raylib)
            var frameTime = Raylib.GetFrameTime();
            // Optional: Clamp, damit bei Rucklern die Physik nicht explodiert
            if (frameTime > 0.1f) frameTime = 0.1f;
            
            world.Update(frameTime);

            // 2. Zeichnen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            
            Raylib.BeginMode2D(camera);

            // Boden zeichnen
            var points = world.Points; // Das geht nur mit der Änderung aus Schritt 2!
            for (var i = 0; i < points.Count - 1; i++)
            {
                var p1 = points[i];
                var p2 = points[i+1];
                // Zeichne dicke grüne Linie für den Boden
                Raylib.DrawLineEx(
                    new Vector2(p1.X, p1.Y), 
                    new Vector2(p2.X, p2.Y), 
                    4.0f, Color.DarkGreen
                );
                // Zeichne kleine Punkte an den Ecken
                Raylib.DrawCircleV(new Vector2(p1.X, p1.Y), 5, Color.DarkGreen);
            }

            
            // Hindernisse zeichnen (Dunkelgrau)
            foreach (var obs in world.Obstacles)
            {
                // Position ist Unten-Links -> Raylib zeichnet von Oben-Links
                // Also Y - Höhe
                Raylib.DrawRectangle(
                    (int)obs.Position.X, 
                    (int)(obs.Position.Y - obs.Size.Y), 
                    (int)obs.Size.X, 
                    (int)obs.Size.Y, 
                    Color.DarkGray
                );
            }

// Spieler zeichnen (angepasst an die echte Größe)
            Raylib.DrawRectangle(
                (int)player.Position.X, 
                (int)(player.Position.Y - player.Size.Y), 
                (int)player.Size.X, 
                (int)player.Size.Y, 
                Color.Red
            );
            
            
            // Spieler zeichnen (Rotes Rechteck, mittig über dem Punkt)
            // Wir müssen die Position leicht anpassen, da Raylib von oben links zeichnet
            var playerPos = player.Position;
            Raylib.DrawRectangle((int)playerPos.X - 10, (int)playerPos.Y - 20, 20, 20, Color.Red);

            Raylib.EndMode2D();

            // UI Overlay
            Raylib.DrawText($"Pos: {player.Position.X:F1} / {player.Position.Y:F1}", 10, 10, 20, Color.Black);
            if (player.IsGrounded) Raylib.DrawText("BODEN", 10, 30, 20, Color.Green);
            camera.Target = new Vector2(player.Position.X, player.Position.Y);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}