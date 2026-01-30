using System.Numerics;

namespace SpielTools;

public class Player : Entity
{
    private const float Gravity = 200.0f; 
    private const float JumpStrength = -120.0f;

    public override void Update(float deltaTime, World world)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);

        base.Update(deltaTime, world);

        CheckGroundCollision(world);

        if (!(Position.Y > 500)) return;
        Position = new Vector2(0, 0);
        Velocity = Vector2.Zero;
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

    public void Jump()
    {
        if (!IsGrounded) return;
        Velocity = Velocity with { Y = JumpStrength };
        IsGrounded = false;
    }

    public void MoveRight() => Velocity = new Vector2(50, Velocity.Y);
    public void MoveLeft() => Velocity = new Vector2(-50, Velocity.Y);
    public void StopMoving() => Velocity = new Vector2(0, Velocity.Y);
}