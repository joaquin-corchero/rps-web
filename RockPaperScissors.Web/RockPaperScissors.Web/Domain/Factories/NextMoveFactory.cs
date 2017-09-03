using RockPaperScissors.Web.Models;
using RockPaperScissors.Web.Models.Domain.MoveGenerators;
using System;

namespace RockPaperScissors.Web.Domain.Factories
{
    public interface INextMoveFactory
    {
        MoveTypes Generate(IPlayer player);
    }

    public class NextMoveFactory : INextMoveFactory
    {
        readonly IRandomGenerator _randomGenerator;
        readonly ITacticalGenerator _tacticalGenerator;

        public NextMoveFactory(IRandomGenerator randomGenerator, ITacticalGenerator tacticalGenerator)
        {
            _randomGenerator = randomGenerator;
            _tacticalGenerator = tacticalGenerator;
        }

        public NextMoveFactory(): this(new RandomGenerator(), new TacticalGenerator()) { }


        public MoveTypes Generate(IPlayer player)
        {
            switch(player.Playertype)
            {
                case PlayerTypes.Human:
                    return MoveTypes.Rock;
                case PlayerTypes.Random:
                    return _randomGenerator.GetMove();
                case PlayerTypes.Tactical:
                    return _tacticalGenerator.GetMove(player as TacticalPlayer);
            }

            throw new ArgumentException("No implementation fo the the type of player passed");
        }
    }
}