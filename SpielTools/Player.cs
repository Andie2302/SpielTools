using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f;
    private const float MoveSpeed = 60.0f; // NEU: Geschwindigkeit für Laufen

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
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);

        base.Update(deltaTime, world);

        CheckGroundCollision(world);

        // Reset bei Absturz
        if (Position.Y > 500) 
        {
            Position = new Vector2(0, 0);
            Velocity = Vector2.Zero;
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
        }

        // Wir übernehmen die horizontale Geschwindigkeit, wenn wir nicht gerade springen
        // (Damit man beim Springen nicht sofort stoppt, wenn man die Taste loslässt, 
        // könnte man das hier noch verfeinern, aber für den Anfang reicht das)
        if (action != GameAction.Jump)
        {
             Velocity = new Vector2(targetSpeedX, Velocity.Y);
        }
    }

    private void CheckGroundCollision(World world)
    {
        var groundY = world.GetGroundY(Position.X);

        if (Position.Y >= groundY && Velocity.Y >= 0)
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