using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Models;
using Should.Fluent;
using System.Collections.Generic;

namespace RockPaperScissors.Web.Tests.Domain
{
    [TestFixture]
    public class When_working_with_the_game_referee
    {
        protected IGameReferee _referee;
        protected Game _game;

        [SetUp]
        public void Init()
        {
            _referee = new GameReferee();
            _game = Game.Start(Game.DefaultNumberOfPlayers, Game.DefaultNumberOfTries);
        }
    }

    [TestFixture]
    public class And_getting_the_winner : When_working_with_the_game_referee
    {

        List<MoveTypes> _win_moves = new List<MoveTypes> { MoveTypes.Rock, MoveTypes.Scissors};
        List<MoveTypes> _losing_moves = new List<MoveTypes> { MoveTypes.Rock, MoveTypes.Paper };
        List<MoveTypes> _draw_moves = new List<MoveTypes> { MoveTypes.Rock, MoveTypes.Rock };
        int? _result;

        void Execute()
        {
            _result = _referee.GetWinner(_game);
        }

        [Test]
        public void Null_is_returned_if_there_are_no_tries()
        {
            Execute();
            _result.Should().Be.Null();
        }

        [Test]
        public void The_winner_is_returned_for_3_wins()
        {
            var expected = 0;
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[1].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[2].SetWinner(expected);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void The_winner_is_returned_for_2_wins_and_1_lost_game()
        {
            var expected = 0;
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[1].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_losing_moves));
            _game.Tries[2].SetWinner(1);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void The_winner_is_returned_for_2_wins_and_1_draw()
        {
            var expected = 0;
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[1].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[2].SetWinner(Game.DrawPlayerIndex);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void The_winner_is_returned_for_1_wins_and_2_draws()
        {
            var expected = 0;
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[1].SetWinner(Game.DrawPlayerIndex);
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[2].SetWinner(Game.DrawPlayerIndex);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void The_winner_is_returned_for_2_wins_only()
        {
            var expected = 0;
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[1].SetWinner(expected);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void Null_is_returned_for_3_draws()
        {
            var expected = Game.DrawPlayerIndex;
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[0].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[1].SetWinner(expected);
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[2].SetWinner(expected);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void Draw_is_returned_after_draw_win_lose()
        {
            _game.Tries.Add(Move.Generate(_draw_moves));
            _game.Tries[0].SetWinner(Game.DrawPlayerIndex);
            _game.Tries.Add(Move.Generate(_win_moves));
            _game.Tries[0].SetWinner(0);
            _game.Tries.Add(Move.Generate(_losing_moves));
            _game.Tries[1].SetWinner(1);

            Execute();

            _result.Should().Equal(Game.DrawPlayerIndex);
        }
    }
}