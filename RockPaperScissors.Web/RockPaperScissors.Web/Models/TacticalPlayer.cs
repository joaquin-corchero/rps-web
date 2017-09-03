using RockPaperScissors.Web.Domain;
using System.Collections.Generic;

namespace RockPaperScissors.Web.Models
{
    public class TacticalPlayer : IPlayer
    {
        public List<MoveTypes> Moves { get; internal set; }

        public PlayerTypes Playertype => PlayerTypes.Tactical;

        public TacticalPlayer()
        {
            Moves = new List<MoveTypes>();
        }
    }
}