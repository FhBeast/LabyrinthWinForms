using Labyrinth.Model;

namespace Labyrinth.Controller;

internal static class GameplayManager
{
    public static bool LevelIsComplete(List<Coin> coins)
    {
        if (coins is null)
        {
            throw new ArgumentNullException($"{nameof(coins)}");
        }

        if (coins.Count < 1) return true;
        return false;
    }
}
