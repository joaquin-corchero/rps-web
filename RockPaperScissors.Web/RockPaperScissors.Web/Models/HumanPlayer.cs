using RockPaperScissors.Web.Domain;

namespace RockPaperScissors.Web.Models
{
    public class HumanPlayer : IPlayer
    {
        public PlayerTypes Playertype => PlayerTypes.Human;
    }
}