using RockPaperScissors.Web.Domain;

namespace RockPaperScissors.Web.Models
{
    public class RandomPlayer : IPlayer
    {
        public PlayerTypes Playertype => PlayerTypes.Random;
    }
}