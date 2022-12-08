using Labyrinth.Model;
using Labyrinth.Util;

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

    public static bool CoinIsPicked(List<Coin> coins, Player player)
    {
        if (coins is null)
        {
            throw new ArgumentNullException($"{nameof(coins)}");
        }

        if (player is null)
        {
            throw new ArgumentNullException($"{nameof(player)}");
        }

        if (CollisionVerificator.Collision(player, coins.Cast<BasePlayableEntity>().ToList()))
        {
            coins.RemoveAll(x => x.IsPicked);
            return true;
        }

        return false;
    }

    public static bool GameIsOver(Enemy enemy, Player player)
    {
        if (enemy is null)
        {
            throw new ArgumentNullException($"{nameof(enemy)}");
        }

        if (player is null)
        {
            throw new ArgumentNullException($"{nameof(player)}");
        }

        if (CollisionVerificator.Collision(player, enemy))
        {
            return true;
        }

        return false;
    }
}
