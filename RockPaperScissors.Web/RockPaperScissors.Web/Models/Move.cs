using RockPaperScissors.Web.Domain;
using System.Collections.Generic;

namespace RockPaperScissors.Web.Models
{
    public class Move
    {
        public int? WinnerIndex { get; private set; }

        public List<MoveTypes> MoveTypes { get; private set; }

        Move(List<MoveTypes> moveTypes)
        {
            ResetMoves(moveTypes);
        }

        public static Move Generate(List<MoveTypes> moveTypes)
        {
            return new Move(moveTypes);
        }

        public void SetWinner(int winnerIndex)
        {
            WinnerIndex = winnerIndex;
        }

        internal void ResetMoves(List<MoveTypes> moveTypes)
        {
            MoveTypes = moveTypes;
        }
    }
}