using RockPaperScissors.Web.Domain;
using System;
using System.Linq;

namespace RockPaperScissors.Web.Models.Domain.MoveGenerators
{
    public interface ITacticalGenerator
    {
        MoveTypes GetMove(TacticalPlayer player);
    }

    public class TacticalGenerator : ITacticalGenerator
    {
        readonly IRandomGenerator _randomGenerator;

        public TacticalGenerator(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public TacticalGenerator() : this(new RandomGenerator()) { }


        public MoveTypes GetMove(TacticalPlayer player)
        {
            var nextMove = GetNextMove(player);

            player.Moves.Add(nextMove);

            return nextMove;
        }

        private MoveTypes GetNextMove(TacticalPlayer player)
        {
            var previousMoves = player.Moves;
            if (!player.Moves.Any())
            {
                return _randomGenerator.GetMove();
            }

            switch (player.Moves.LastOrDefault())
            {
                case MoveTypes.Paper:
                    return MoveTypes.Scissors;
                case MoveTypes.Rock:
                    return MoveTypes.Paper;
                case MoveTypes.Scissors:
                    return MoveTypes.Rock;
            }

            throw new ArgumentException("No move defined agains the last move");
        }
    }
}