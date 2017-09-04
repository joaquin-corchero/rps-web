using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using Should.Fluent;
using System.Collections.Generic;

namespace RockPaperScissors.Web.Tests.Domain
{
    public abstract class When_working_with_the_tow_player_move_referee_calculator
    {
        protected IMoveReferee _referee = new TwoPlayerMoveReferee();
    }

    [TestFixture]
    public class And_calculating_the_winner : When_working_with_the_tow_player_move_referee_calculator
    {
        MoveTypes _player1;
        MoveTypes _player2;
        int _winnerIndex;

        private void Execute()
        {

            _winnerIndex = _referee.GetWinnerIndex(new List<MoveTypes> { _player1, _player2});
        }
        
        [Test]
        public void Minus_1_is_returned_if_both_do_paper()
        {
            _player1 = MoveTypes.Paper;
            _player2 = MoveTypes.Paper;
            Execute();

            _winnerIndex.Should().Equal(Game.DrawPlayerIndex);
        }

        [Test]
        public void Minus_1_is_returned_if_both_do_rock()
        {
            _player1 = MoveTypes.Rock;
            _player2 = MoveTypes.Rock;
            Execute();

            _winnerIndex.Should().Equal(Game.DrawPlayerIndex);
        }

        [Test]
        public void Minus_1_is_returned_if_both_do_scissor()
        {
            _player1 = MoveTypes.Scissors;
            _player2 = MoveTypes.Scissors;
            Execute();

            _winnerIndex.Should().Equal(Game.DrawPlayerIndex);
        }

        [Test]
        public void One_is_returned_with_paper_scissors()
        {
            _player1 = MoveTypes.Paper;
            _player2 = MoveTypes.Scissors;
            Execute();

            _winnerIndex.Should().Equal(1);
        }

        [Test]
        public void Zero_is_returned_with_scissors_paper()
        {
            _player1 = MoveTypes.Scissors;
            _player2 = MoveTypes.Paper;
            Execute();

            _winnerIndex.Should().Equal(0);
        }

        [Test]
        public void One_is_returned_with_rock_scissors()
        {
            _player1 = MoveTypes.Rock;
            _player2 = MoveTypes.Scissors;
            Execute();

            _winnerIndex.Should().Equal(0);
        }

        [Test]
        public void Zero_is_returned_with_scissors_rock()
        {
            _player1 = MoveTypes.Scissors;
            _player2 = MoveTypes.Rock;
            Execute();

            _winnerIndex.Should().Equal(1);
        }

        [Test]
        public void One_is_returned_with_rock_paper()
        {
            _player1 = MoveTypes.Rock;
            _player2 = MoveTypes.Paper;
            Execute();

            _winnerIndex.Should().Equal(1);
        }

        [Test]
        public void Zero_is_returned_with_paper_rock()
        {
            _player1 = MoveTypes.Paper;
            _player2 = MoveTypes.Rock;
            Execute();

            _winnerIndex.Should().Equal(0);
        }



    }
}
