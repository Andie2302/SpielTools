using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 9.81f * 100f; // Pixel pro Sekunde^2

    public override void Update(float deltaTime, World world)
    {
        // 1. Schwerkraft anwenden, wenn nicht am Boden
        if (!IsGrounded)
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);
        }

        // 2. Basis-Bewegung ausführen
        base.Update(deltaTime, world);

        // 3. Boden-Kollision (Grob-Logik)
        float groundY = world.GetGroundY(Position.X);
        if (Position.Y >= groundY)
        {
            Position = new Vector2(Position.X, groundY);
            Velocity = new Vector2(Velocity.X, 0);
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
}