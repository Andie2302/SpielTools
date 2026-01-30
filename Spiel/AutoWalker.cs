using SpielTools;

namespace Spiel;

class AutoWalker : IController
{
    public GameAction GetAction(Player p, World w) => GameAction.MoveRight;
}