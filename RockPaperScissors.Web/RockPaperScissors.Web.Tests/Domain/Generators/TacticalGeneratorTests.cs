using Moq;
using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Models;
using RockPaperScissors.Web.Models.Domain.MoveGenerators;
using Should.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Web.Tests.Domain.Generators
{
    [TestFixture]
    public class When_working_with_the_tactical_generator
    {
        protected ITacticalGenerator _tacticalGenerator;
        protected Mock<IRandomGenerator> _randomGenerator;

        [SetUp]
        public void Init()
        {
            _randomGenerator = new Mock<IRandomGenerator>();
            _tacticalGenerator = new TacticalGenerator(_randomGenerator.Object);
        }
    }

    [TestFixture]
    public class And_getting_a_tactical_move : When_working_with_the_tactical_generator
    {
        MoveTypes _result;
        List<MoveTypes> _moveTypes;
        TacticalPlayer _tacticalPlayer;

        [SetUp]
        public new void Init()
        {
            base.Init();
            _moveTypes = Enum.GetValues(typeof(MoveTypes)).Cast<MoveTypes>().ToList();
            _tacticalPlayer = new TacticalPlayer();
        }

        void Execute()
        {
            _result = _tacticalGenerator.GetMove(_tacticalPlayer);
        }

        [Test]
        public void First_move_is_a_random_move()
        {
            var expected = MoveTypes.Scissors;
            _randomGenerator.Setup(r => r.GetMove()).Returns(expected);

            Execute();

            _randomGenerator.Verify(r => r.GetMove(), Times.Once);
            _result.Should().Equal(expected);
        }

        [TestCase(MoveTypes.Paper, MoveTypes.Scissors)]
        [TestCase(MoveTypes.Scissors, MoveTypes.Rock)]
        [TestCase(MoveTypes.Rock, MoveTypes.Paper)]
        public void After_first_move_moves_are_to_beat_the_previous_one(MoveTypes previous, MoveTypes expected)
        {
            _tacticalPlayer.Moves.Add(previous);

            Execute();

            _randomGenerator.Verify(r => r.GetMove(), Times.Never);

            _result.Should().Equal(expected);
        }
    }
}
