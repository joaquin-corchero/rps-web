using System.Collections.Generic;

namespace RockPaperScissors.Web.Models
{
    public class Game
    {
        public const int DefaultNumberOfTries = 3;
        public const int DefaultNumberOfPlayers = 2;

        public List<IPlayer> Players { get; private set; }
        public List<Move> Tries { get; set; }
        public int NumberOfPlayers { get; }
        public int NumberOfTries { get; }
        public int? Winner { get; private set; }
        public bool HasFinished => Winner.HasValue;

        Game(int numberOfPlayers, int numberOfTries)
        {
            NumberOfPlayers = numberOfPlayers;
            NumberOfTries = numberOfTries;
            Players = new List<IPlayer>(numberOfPlayers);
            Tries = new List<Move>(numberOfTries);
        }

        public static Game Start (int numberOfPlayers, int numberOfTries)
        {
            return new Game(numberOfPlayers, numberOfTries);
        }

        public void SetWinner(int? winner)
        {
            Winner = winner;
        }
    }
}