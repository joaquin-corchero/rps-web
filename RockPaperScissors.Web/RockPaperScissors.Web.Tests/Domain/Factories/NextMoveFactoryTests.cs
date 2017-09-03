using Moq;
using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Domain.Factories;
using RockPaperScissors.Web.Models;
using RockPaperScissors.Web.Models.Domain.MoveGenerators;
using Should.Fluent;

namespace RockPaperScissors.Web.Tests.Domain.Factories
{
    [TestFixture]
    public class When_working_with_the_next_moves_factory
    {
        protected INextMoveFactory _factory;
        protected Mock<IRandomGenerator> _randomGenerator;
        protected Mock<ITacticalGenerator> _tacticalGenerator;

        [SetUp]
        public void Init()
        {
            _randomGenerator = new Mock<IRandomGenerator>();
            _tacticalGenerator = new Mock<ITacticalGenerator>();
            _factory = new NextMoveFactory(_randomGenerator.Object, _tacticalGenerator.Object);

        }        
    }

    [TestFixture]
    public class And_getting_a_move : When_working_with_the_next_moves_factory
    {
        IPlayer _player;
        MoveTypes _result;

        void Execute()
        {
            _result = _factory.Generate(_player);
        }


        [Test]
        public void Rock_is_returned_for_the_human_player()
        {
            _player = new HumanPlayer();

            Execute();

            _result.Should().Equal(MoveTypes.Rock);
        }

        [Test]
        public void A_random_move_is_retuned_for_random_player()
        {
            var expected = MoveTypes.Scissors;
            _player = new RandomPlayer();
            _randomGenerator.Setup(r => r.GetMove()).Returns(expected);

            Execute();

            _result.Should().Equal(expected);
        }

        [Test]
        public void A_tactical_move_is_retuned_for_tactical_player()
        {
            var expected = MoveTypes.Paper;
            _player = new TacticalPlayer();
            _tacticalGenerator.Setup(r => r.GetMove(_player as TacticalPlayer)).Returns(expected);

            Execute();

            _result.Should().Equal(expected);
        }
    }
}
