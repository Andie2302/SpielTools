using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    // Schwerkraft: 9.81 m/s² * 10 (Skalierung für Pixel-Welt) 
    // Du kannst diesen Wert anpassen, damit es sich "spritziger" anfühlt.
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f; // Negativ = Nach oben

    public override void Update(float deltaTime, World world)
    {
        // 1. Schwerkraft anwenden (wenn wir nicht fest am Boden stehen)
        // Wir addieren Schwerkraft zur vertikalen Geschwindigkeit (Y)
        // Aber nur, wenn wir fallen oder springen.
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);

        // 2. Die Bewegung ausführen (Basis-Logik von Entity)
        base.Update(deltaTime, world);

        // 3. Boden-Kollision prüfen
        CheckGroundCollision(world);
        
        // 4. Absturz-Reset (Wenn wir in ein Loch gefallen sind)
        if (Position.Y > 500) // 500 ist "ganz tief unten"
        {
            // Reset zum Start (einfache Logik für jetzt)
            Position = new Vector2(0, 0);
            Velocity = Vector2.Zero;
        }
    }

    private void CheckGroundCollision(World world)
    {
        // Wo ist der Boden unter meinen Füßen?
        float groundY = world.GetGroundY(Position.X);

        // Kollision passiert, wenn:
        // a) Meine Füße (Position.Y) tiefer oder gleich dem Boden sind
        // b) Ich mich nach unten bewege (Velocity.Y > 0) -> Wichtig, damit man durch Boden nach oben springen kann!
        if (Position.Y >= groundY && Velocity.Y >= 0)
        {
            // Korrektur: Setze Spieler exakt auf den Boden
            Position = new Vector2(Position.X, groundY);
            
            // Stoppe die Fall-Geschwindigkeit
            Velocity = new Vector2(Velocity.X, 0);
            IsGrounded = true;
        }
        else
        {
            // Wenn wir in der Luft sind oder den Boden verlassen
            IsGrounded = false;
        }
    }

    // Befehle, die später von Tastatur ODER KI kommen
    public void Jump()
    {
        if (IsGrounded)
        {
            Velocity = new Vector2(Velocity.X, JumpStrength);
            IsGrounded = false; // Sofort Status ändern
        }
    }

    public void MoveRight() => Velocity = new Vector2(50, Velocity.Y);
    public void MoveLeft() => Velocity = new Vector2(-50, Velocity.Y);
    public void StopMoving() => Velocity = new Vector2(0, Velocity.Y);
}