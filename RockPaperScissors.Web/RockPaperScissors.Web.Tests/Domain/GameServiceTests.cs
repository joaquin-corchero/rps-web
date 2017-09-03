using Moq;
using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Domain.Factories;
using RockPaperScissors.Web.Models;
using Should.Fluent;
using System;
using System.Collections.Generic;

namespace RockPaperScissors.Web.Tests.Domain
{
    [TestFixture]
    public class When_working_with_the_game_service
    {
        protected GameService _service;
        protected Mock<IPlayerFactory> _playerFactory;
        protected Mock<INextMoveFactory> _movesFactory;
        protected Mock<IMoveReferee> _moveReferee;
        protected Mock<IGameReferee> _gameReferee;

        protected Game _game;
        protected List<PlayerTypes> _playerTypes;        

        [SetUp]
        public void Init()
        {
            _playerFactory = new Mock<IPlayerFactory>();
            _movesFactory = new Mock<INextMoveFactory>();
            _moveReferee = new Mock<IMoveReferee>();
            _gameReferee = new Mock<IGameReferee>();
            _service = new GameService(_playerFactory.Object, _movesFactory.Object, _moveReferee.Object, _gameReferee.Object);
            _game = Game.Start(Game.DefaultNumberOfPlayers, Game.DefaultNumberOfTries);
        }

        protected void SetupPlayerFactory()
        {
            _playerFactory.Setup(f => f.Generate(PlayerTypes.Human)).Returns(new HumanPlayer());
            _playerFactory.Setup(f => f.Generate(PlayerTypes.Random)).Returns(new RandomPlayer());
        }
    }

    [TestFixture]
    public class And_resetting_the_game : When_working_with_the_game_service
    {
        void Execute()
        {
            _service.ResetGame(_game);
        }

        [Test]
        public void Then_the_game_is_reset()
        {
            _game.Players.Should().Be.Empty();
            _game.Tries.Should().Be.Empty();
            _game.Winner.Should().Be.Null();
            _game.HasFinished.Should().Be.False();
        }
    }

    [TestFixture]
    public class And_setting_up_the_players: When_working_with_the_game_service
    { 
        [SetUp]
        public new void Init()
        {
            base.Init();
            _playerTypes = new List<PlayerTypes> { PlayerTypes.Human, PlayerTypes.Random };
            SetupPlayerFactory();
        }

        void Execute()
        {
            _service.SetupPlayers(_game, _playerTypes);
        }

        [Test]
        public void The_playerFactory_is_called_for_each_player_type()
        {
            Execute();

            _playerFactory.Verify(f => f.Generate(_playerTypes[0]), Times.Once);
            _playerFactory.Verify(f => f.Generate(_playerTypes[1]), Times.Once);
        }

        [Test]
        public void The_players_are_added_to_the_game()
        {
            Execute();

            _game.Players.Count.Should().Equal(2);
            _game.Players[0].Should().Be.OfType<HumanPlayer>();
            _game.Players[1].Should().Be.OfType<RandomPlayer>();
        }

        [Test]
        public void Exception_is_thrown_if_player_types_differ_from_game_number_of_players()
        {
            _playerTypes = new List<PlayerTypes> { PlayerTypes.Human, PlayerTypes.Random };
            _game = Game.Start(1, 2);
            Assert.Throws(typeof(ArgumentException), new TestDelegate(Execute));
        }
    }

    [TestFixture]
    public class And_making_the_next_move : When_working_with_the_game_service
    { 
        [SetUp]
        public new void Init()
        {
            base.Init();
            _playerTypes = new List<PlayerTypes> { PlayerTypes.Human, PlayerTypes.Random };
            SetupPlayerFactory();
            _service.SetupPlayers(_game, _playerTypes);
            _movesFactory.Setup(m => m.Generate(_game.Players[0] as HumanPlayer)).Returns(MoveTypes.Paper);
            _movesFactory.Setup(m => m.Generate(_game.Players[1] as RandomPlayer)).Returns(MoveTypes.Scissors);
        }

        void Execute()
        {
            _service.NextMove(_game);
        }

        [Test]
        public void A_move_for_each_player_is_created()
        {
            Execute();

            _movesFactory.Verify(f => f.Generate(_game.Players[0] as HumanPlayer), Times.Once);
            _movesFactory.Verify(f => f.Generate(_game.Players[1] as RandomPlayer), Times.Once);
        }

        [Test]
        public void A_try_is_created()
        {
            var moves = 0;
            _game.Tries.Count.Should().Equal(moves);

            Execute();

            _game.Tries.Count.Should().Equal(moves + 1);
            _game.Tries[0].MoveTypes[0].Should().Equal(MoveTypes.Paper);
            _game.Tries[0].MoveTypes[1].Should().Equal(MoveTypes.Scissors);
        }

        [Test]
        public void No_tries_are_added_then_maximum_is_reached()
        {
            _game.Tries.Count.Should().Equal(0);
            _moveReferee.Setup(c => c.GetWinnerIndex(It.IsAny<List<MoveTypes>>())).Returns(1);

            Execute();
            _service.SetWinnerForLastTry(_game, new List<MoveTypes> { MoveTypes.Paper, MoveTypes.Paper});
            Execute();
            _service.SetWinnerForLastTry(_game, new List<MoveTypes> { MoveTypes.Paper, MoveTypes.Rock });
            Execute();
            _service.SetWinnerForLastTry(_game, new List<MoveTypes> { MoveTypes.Paper, MoveTypes.Scissors });
            Execute();
            Execute();

            _game.Tries.Count.Should().Equal(Game.DefaultNumberOfTries);
        }

        [Test]
        public void No_tries_are_added_if_the_last_move_does_not_have_a_winner()
        {
            _game.Tries.Count.Should().Equal(0);
            _moveReferee.Setup(c => c.GetWinnerIndex(It.IsAny<List<MoveTypes>>())).Returns(1);

            Execute();
            Execute();

            _game.Tries.Count.Should().Equal(1);
        }
    }

    [TestFixture]
    public class And_setting_the_winner_for_the_last_try : When_working_with_the_game_service
    {
        [SetUp]
        public new void Init()
        {
            base.Init();
            _playerTypes = new List<PlayerTypes> { PlayerTypes.Human, PlayerTypes.Random };
            SetupPlayerFactory();
            _service.SetupPlayers(_game, _playerTypes);
            _movesFactory.Setup(m => m.Generate(_game.Players[0] as HumanPlayer)).Returns(MoveTypes.Paper);
            _movesFactory.Setup(m => m.Generate(_game.Players[1] as RandomPlayer)).Returns(MoveTypes.Scissors);
        }

        void Execute()
        {
            _service.SetWinnerForLastTry(_game, new List<MoveTypes> { MoveTypes.Paper, MoveTypes.Scissors});
        }

        [Test]
        public void The_index_of_the_winner_player_is_set()
        {
            int expected = 1;
            _service.NextMove(_game);
            _game.Tries[0].MoveTypes.Count.Should().Equal(2);
            _game.Tries[0].WinnerIndex.Should().Be.Null();
            _moveReferee.Setup(c => c.GetWinnerIndex(_game.Tries[0].MoveTypes)).Returns(expected);

            Execute();

            _game.Tries[0].WinnerIndex.Should().Equal(expected);
        }
    }

    [TestFixture]
    public class And_trying_to_get_a_winner_3_tries : When_working_with_the_game_service
    {
        int? _winner;

        void Execute()
        {
            _service.TryToSetGameWinner(_game);
        }

        [Test]
        public void If_there_is_no_winner_the_game_is_not_finished()
        {
            _winner = null;
            _gameReferee.Setup(r => r.GetWinner(_game)).Returns(_winner);

            Execute();

            _game.Winner.Should().Equal(_winner);
            _game.HasFinished.Should().Be.False();
        }

        [Test]
        public void If_there_is_a_draw_the_game_is_finished()
        {
            _winner = -1;
            _gameReferee.Setup(r => r.GetWinner(_game)).Returns(_winner);

            Execute();

            _game.Winner.Should().Be.Equals(_winner);
            _game.HasFinished.Should().Be.True();
        }

        [Test]
        public void If_there_is_a_winner_the_game_is_finished()
        {
            _winner = 0;
            _gameReferee.Setup(r => r.GetWinner(_game)).Returns(_winner);

            Execute();

            _game.Winner.Should().Be.Equals(_winner);
            _game.HasFinished.Should().Be.True();
        }
    }

}
