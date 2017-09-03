using RockPaperScissors.Web.Models;
using System;

namespace RockPaperScissors.Web.Domain.Factories
{
    public interface IPlayerFactory
    {
        IPlayer Generate(PlayerTypes playerType);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Generate(PlayerTypes playerType)
        {
            switch (playerType)
            {
                case PlayerTypes.Human:
                    return new HumanPlayer();
                case PlayerTypes.Random:
                    return new RandomPlayer();
                case PlayerTypes.Tactical:
                    return new TacticalPlayer();
            }

            throw new Exception($"No player implements the player type {playerType.ToString()}");
        }
    }
}