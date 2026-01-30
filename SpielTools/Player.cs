using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f;
    private const float MoveSpeed = 60.0f;

    public Player() { Size = new Vector2(20, 20); }
    public IController? Controller { get; set; }

    public override void Update(float deltaTime, World world)
    {
        // Wenn Game Over ist, macht der Spieler NICHTS mehr.
        if (world.IsGameOver) return;

        // 1. Input
        if (Controller != null)
        {
            var action = Controller.GetAction(this, world);
            ApplyInput(action);
        }

        // 2. Physik
        Velocity = Velocity with { Y = Velocity.Y + Gravity * deltaTime };
        base.Update(deltaTime, world);

        CheckGroundCollision(world);
        CheckObstacleCollision(world);

        // 3. ABSTURZ (Der Fix)
        // Statt "Position = 0,0" setzen wir den Status auf Game Over.
        if (Position.Y > 500) 
        {
            world.IsGameOver = true; 
        }
    }
    
    // ... CheckObstacleCollision, CheckGroundCollision und ApplyInput bleiben gleich wie in deinem Upload ...
    // Wichtig: In CheckObstacleCollision auch "world.IsGameOver = true;" setzen, statt nur Console.Write.
    private void CheckObstacleCollision(World world)
    {
        // ... deine Rechteck-Berechnung ...
        // if (Treffer) { world.IsGameOver = true; }
        // (Hier den Code aus deinem Upload nutzen, aber Velocity-Setzen durch IsGameOver ersetzen)
    }

    // Deine ApplyInput & CheckGroundCollision Methoden von vorhin hier einfügen...
    private void ApplyInput(GameAction action)
    {
         // Code aus deinem Upload
         float targetSpeedX = 0;
         switch (action)
         {
             case GameAction.MoveLeft: targetSpeedX = -MoveSpeed; break;
             case GameAction.MoveRight: targetSpeedX = MoveSpeed; break;
             case GameAction.Jump:
                 if (IsGrounded) { Velocity = Velocity with { Y = JumpStrength }; IsGrounded = false; }
                 break;
         }
         if (action != GameAction.Jump) Velocity = Velocity with { X = targetSpeedX };
    }

    private void CheckGroundCollision(World world)
    {
        // Code aus deinem Upload
        var groundY = world.GetGroundY(Position.X);
        if (Position.Y >= groundY && Velocity.Y >= 0)
        {
            Position = Position with { Y = groundY };
            Velocity = Velocity with { Y = 0 };
            IsGrounded = true;
        }
        else IsGrounded = false;
    }
}