using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Domain.Factories;
using RockPaperScissors.Web.Models;
using Should.Fluent;

namespace RockPaperScissors.Web.Tests.Domain.Factories
{
    [TestFixture]
    public class When_working_with_the_player_factory
    {
        protected IPlayerFactory _service;
        protected Game _game;

        [SetUp]
        public void Init()
        {
            _service = new PlayerFactory();
        }
    }

    [TestFixture]
    public class And_generating_a_player : When_working_with_the_player_factory
    {
        PlayerTypes _playerType;
        IPlayer _result;

        void Execute()
        {
            _result = _service.Generate(_playerType);
        }

        [Test]
        public void A_human_player_is_returned()
        {
            _playerType = PlayerTypes.Human;
            Execute();

            _result.GetType().Should().Equal(typeof(HumanPlayer));
        }

        [Test]
        public void A_random_player_is_returned()
        {
            _playerType = PlayerTypes.Random;
            Execute();

            _result.GetType().Should().Equal(typeof(RandomPlayer));
        }

        [Test]
        public void A_tactical_player_is_returned()
        {
            _playerType = PlayerTypes.Tactical;
            Execute();

            _result.GetType().Should().Equal(typeof(TacticalPlayer));
        }

    }
}
