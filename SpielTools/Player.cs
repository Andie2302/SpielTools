using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f;
    private const float MoveSpeed = 60.0f; // NEU: Geschwindigkeit für Laufen

    public Player()
    {
        // Wir definieren: Der Spieler ist logisch 20x20 Pixel groß
        Size = new Vector2(20, 20);
    }
    
    // Hier dockt später die KI oder deine Tastatur an
    public IController? Controller { get; set; }

    public override void Update(float deltaTime, World world)
    {
        // 1. NEU: Controller fragen "Was soll ich tun?"
        if (Controller != null)
        {
            var action = Controller.GetAction(this, world);
            ApplyInput(action);
        }

        // 2. Physik (Schwerkraft)
        Velocity = Velocity with { Y = Velocity.Y + Gravity * deltaTime };

        base.Update(deltaTime, world);

        CheckGroundCollision(world);
        CheckObstacleCollision(world);

        // Reset bei Absturz
        if (Position.Y > 500) 
        {
            Position = new Vector2(0, 0);
            Velocity = Vector2.Zero;
        }
    }

    private void CheckObstacleCollision(World world)
    {
        // Rechteck-Kollision (AABB)
        // Unser Spieler:
        // Links: Position.X - Size.X/2 (da wir ihn mittig zeichnen wollen, siehe Renderer)
        // Rechts: Position.X + Size.X/2
        // Unten: Position.Y
        // Oben: Position.Y - Size.Y
        
        // Um es einfach zu halten, nehmen wir an: Position ist UNTEN LINKS.
        // Player Rechteck:
        var pLeft = Position.X;
        var pRight = Position.X + Size.X;
        var pBottom = Position.Y;
        var pTop = Position.Y - Size.Y;

        foreach (var obs in world.Obstacles)
        {
            // Hindernis Rechteck:
            var oLeft = obs.Position.X;
            var oRight = obs.Position.X + obs.Size.X;
            var oBottom = obs.Position.Y;
            var oTop = obs.Position.Y - obs.Size.Y;

            // Überschneiden sich die Rechtecke?
            var collisionX = pRight > oLeft && pLeft < oRight;
            var collisionY = pBottom > oTop && pTop < oBottom;

            if (!collisionX || !collisionY) continue;
            // TREFFER! 
            // Einfache Reaktion: Rückstoß und Stop
            Velocity = new Vector2(-100, -150); // Nach hinten oben schleudern
            // Später: Score abziehen oder Game Over
            Console.WriteLine("AUTSCH! Hindernis berührt.");
        }
    }
    
    // Diese Methode setzt die Befehle in Bewegung um
    private void ApplyInput(GameAction action)
    {
        float targetSpeedX = 0;

        switch (action)
        {
            case GameAction.MoveLeft:
                targetSpeedX = -MoveSpeed;
                break;
            case GameAction.MoveRight:
                targetSpeedX = MoveSpeed;
                break;
            case GameAction.Jump:
                if (IsGrounded)
                {
                    Velocity = Velocity with { Y = JumpStrength };
                    IsGrounded = false;
                }
                break;
            case GameAction.Idle:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }

        // Wir übernehmen die horizontale Geschwindigkeit, wenn wir nicht gerade springen
        // (Damit man beim Springen nicht sofort stoppt, wenn man die Taste loslässt, 
        // könnte man das hier noch verfeinern, aber für den Anfang reicht das)
        if (action != GameAction.Jump)
        {
             Velocity = Velocity with { X = targetSpeedX };
        }
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
        else
        {
            IsGrounded = false;
        }
    }
}