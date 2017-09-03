using RockPaperScissors.Web.Models;
using System.Linq;

namespace RockPaperScissors.Web.Domain
{
    public interface IGameReferee
    {
        int? GetWinner(Game game);
    }

    public class GameReferee : IGameReferee
    {
        public int? GetWinner(Game game)
        {
            if (game.Tries.Count == 0)
                return null;

            var wonTries = game.Tries.Where(t => t.WinnerIndex > -1)
                .GroupBy(
                    t => t.WinnerIndex,
                    (key, g) => new { WinnerIndex = key, NumberOfWins = g.Count() }
                );

            var numberOfDraws = game.Tries.Where(t => t.WinnerIndex == -1).Count();
            var minimumWinsToWin = ((game.NumberOfTries - numberOfDraws) / 2) + 1;

            if (wonTries.Any(t => t.NumberOfWins >= minimumWinsToWin))
                return wonTries.OrderByDescending(t => t.NumberOfWins).First().WinnerIndex;

            if (IsEndOfGameWithNoWinner(game))
                return -1;

            return null;
        }

        bool IsEndOfGameWithNoWinner(Game game)
        {
            return game.NumberOfTries == game.Tries.Count;
        }
    }
}