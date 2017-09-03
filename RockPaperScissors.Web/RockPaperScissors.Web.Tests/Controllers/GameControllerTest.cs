using Moq;
using NUnit.Framework;
using RockPaperScissors.Web.Controllers;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Models;
using Should.Fluent;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RockPaperScissors.Web.Tests.Controllers
{
    [TestFixture]
    public class When_working_with_the_game_controller
    {
        protected Mock<IGameService> _gameService;
        protected GameController _controller;
        protected ActionResult _actionResult;
        protected Game _game;

        [SetUp]
        public void Init()
        {
            _game = Game.Start(Game.DefaultNumberOfPlayers, Game.DefaultNumberOfTries);
            _gameService = new Mock<IGameService>();
            _controller = new GameController(_gameService.Object);
        }
    }

    [TestFixture]
    public class And_starting_a_game : When_working_with_the_game_controller
    {
        private void Execute()
        {
            _actionResult = _controller.Index(null);
        }

        [Test]
        public void The_game_is_initialized()
        {
            var expected = _game;
            Execute();

            var actual = (_actionResult as ViewResult).Model as Game;

            actual.Should().Be.Equals(expected);
        }
    }


    [TestFixture]
    public class And_setting_the_type_of_players : When_working_with_the_game_controller
    {
        List<PlayerTypes> _playerTypes;

        [SetUp]
        public new void Init()
        {
            base.Init();
            _playerTypes = new List<PlayerTypes> { PlayerTypes.Random, PlayerTypes.Tactical };
        }

        void Execute()
        {
            _actionResult = _controller.Index(_game, _playerTypes);
        }

        [Test]
        public void The_players_are_setup()
        {
            Execute();

            _gameService.Verify(s => s.SetupPlayers(_game, _playerTypes), Times.Once);
        }

        [Test]
        public void User_is_redirected_to_play()
        {
            Execute();
            (_actionResult as RedirectToRouteResult)
                .RouteValues["action"]
                .Should()
                .Equal(nameof(GameController.Play));
        }
    }

    [TestFixture]
    public class And_playing_a_hand : When_working_with_the_game_controller
    {
        void Execute()
        {
            _actionResult = _controller.Play(_game);
        }

        [Test]
        public void Moves_are_executed()
        {
            Execute();
            _gameService.Verify(s => s.NextMove(_game), Times.Once);
        }

        [Test]
        public void The_game_is_returned()
        {
            Execute();

            (_actionResult as ViewResult).Model.Should().Equal(_game);
        }

        [Test]
        public void The_view_is_returned()
        {
            Execute();

            (_actionResult as ViewResult).ViewName.Should().Be.Empty();
        }
    }

    [TestFixture]
    public class And_posting_the_move : When_working_with_the_game_controller
    {
        List<MoveTypes> _moveTypes = new List<MoveTypes> { MoveTypes.Paper, MoveTypes.Scissors };

        void Execute()
        {
            _actionResult = _controller.Play(_game, _moveTypes);
        }

        [Test]
        public void The_winner_is_set_for_the_last_try()
        {
            Execute();

            _gameService.Verify(s => s.SetWinnerForLastTry(_game, _moveTypes), Times.Once);
        }

        [Test]
        public void Try_to_get_the_winner()
        {
            Execute();

            _gameService.Verify(s => s.TryToSetGameWinner(_game), Times.Once);
        }

        [Test]
        public void User_is_sent_to_play_again_if_there_is_no_winner()
        {
            Execute();

            (_actionResult as RedirectToRouteResult)
                .RouteValues["action"]
                .Should()
                .Equal(nameof(GameController.Play));
        }

        [Test]
        public void User_is_sent_to_end_if_there_is_a_winner()
        {
            _game.SetWinner(1);
            Execute();

            (_actionResult as RedirectToRouteResult)
                .RouteValues["action"]
                .Should()
                .Equal(nameof(GameController.End));
        }

    }
}
