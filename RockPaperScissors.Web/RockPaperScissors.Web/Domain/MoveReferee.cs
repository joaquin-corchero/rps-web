using System.Collections.Generic;

namespace RockPaperScissors.Web.Domain
{
    public interface IMoveReferee
    {
        int GetWinnerIndex(List<MoveTypes> moveTypes);
    }

    public class TwoPlayerMoveReferee : IMoveReferee
    {
        public int GetWinnerIndex(List<MoveTypes> moveTypes)
        {
            if (moveTypes[0] == moveTypes[1])
                return -1;

            if (PaperVsRock(moveTypes) || RockVsScissors(moveTypes) || ScissorsVsPaper(moveTypes))
                return 0;

            return 1;
        }

        bool ScissorsVsPaper(List<MoveTypes> moveTypes)
        {
            return moveTypes[0] == MoveTypes.Scissors && moveTypes[1] == MoveTypes.Paper;
        }

        bool RockVsScissors(List<MoveTypes> moveTypes)
        {
            return moveTypes[0] == MoveTypes.Rock && moveTypes[1] == MoveTypes.Scissors;
        }

        bool PaperVsRock(List<MoveTypes> moveTypes)
        {
            return moveTypes[0] == MoveTypes.Paper && moveTypes[1] == MoveTypes.Rock;
        }
    }
}