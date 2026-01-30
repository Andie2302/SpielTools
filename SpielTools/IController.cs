namespace SpielTools;

public interface IController
{
    GameAction GetAction(Player player, World world);
}