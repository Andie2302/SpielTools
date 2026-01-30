using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f;
    private const float MoveSpeed = 60.0f;

    public Player()
    {
        Size = new Vector2(20, 20);
    }
    
    public IController? Controller { get; set; }

    public override void Update(float deltaTime, World world)
    {
        // Wenn Game Over ist, bewegen wir uns nicht mehr
        if (world.IsGameOver) return;

        // 1. Controller
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

        // 3. ABSTURZ-CHECK (Geändert!)
        // Statt den Spieler einfach zurückzusetzen, beenden wir das Spiel.
        // Das ist wichtig, damit die KI lernt: "Fallen = Schlecht".
        if (Position.Y > 500) 
        {
            world.IsGameOver = true;
            // Optional: Bestrafung beim Score
            // world.Score -= 100; 
        }
    }

    private void CheckObstacleCollision(World world)
    {
        // ... (Rechteck Berechnung wie gehabt) ...
        var pLeft = Position.X;
        var pRight = Position.X + Size.X;
        var pBottom = Position.Y;
        var pTop = Position.Y - Size.Y;

        foreach (var obs in world.Obstacles)
        {
            var oLeft = obs.Position.X;
            var oRight = obs.Position.X + obs.Size.X;
            var oBottom = obs.Position.Y;
            var oTop = obs.Position.Y - obs.Size.Y;

            var collisionX = pRight > oLeft && pLeft < oRight;
            var collisionY = pBottom > oTop && pTop < oBottom;

            if (collisionX && collisionY)
            {
                // KOLLISION -> GAME OVER
                world.IsGameOver = true;
                // Console.WriteLine("GAME OVER: Hindernis gerammt!");
            }
        }
    }

    // ... ApplyInput und CheckGroundCollision bleiben gleich ...
    private void ApplyInput(GameAction action)
    {
        float targetSpeedX = 0;
        switch (action)
        {
            case GameAction.MoveLeft: targetSpeedX = -MoveSpeed; break;
            case GameAction.MoveRight: targetSpeedX = MoveSpeed; break;
            case GameAction.Jump:
                if (IsGrounded)
                {
                    Velocity = Velocity with { Y = JumpStrength };
                    IsGrounded = false;
                }
                break;
        }
        if (action != GameAction.Jump) Velocity = Velocity with { X = targetSpeedX };
    }

    private void CheckGroundCollision(World world)
    {
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