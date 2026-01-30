using System.Numerics;
using SpielTools;

var world = new World();
world.AddGroundPoint(0, 100);
world.AddGroundPoint(50, 100);

var player = new Player();
player.Position = new Vector2(10, 0);
world.Entities.Add(player);

Console.WriteLine($"START: Spieler Y={player.Position.Y} (In der Luft)");

const float deltaTime = 0.016f;

for (var i = 1; i <= 100; i++)
{
    world.Update(deltaTime);
    if (i % 10 == 0)
    {
        var status = player.IsGrounded ? "AM BODEN" : "FÄLLT";
        Console.WriteLine($"Frame {i}: Y={player.Position.Y:F2} Speed={player.Velocity.Y:F2} -> {status}");
    }
}