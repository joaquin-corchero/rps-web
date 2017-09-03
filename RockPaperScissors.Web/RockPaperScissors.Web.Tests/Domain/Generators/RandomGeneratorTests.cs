using NUnit.Framework;
using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Models.Domain.MoveGenerators;
using Should.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Web.Tests.Domain.Generators
{
    [TestFixture]
    public class When_working_with_the_random_generator
    {
        protected IRandomGenerator _randomGenerator;

        [SetUp]
        public void Init()
        {
            _randomGenerator = new RandomGenerator();
        }
    }

    [TestFixture]
    public class And_getting_a_move : When_working_with_the_random_generator
    {
        MoveTypes _result;
        List<MoveTypes> _moveTypes;

        [SetUp]
        public new void Init()
        {
            base.Init();
            _moveTypes = Enum.GetValues(typeof(MoveTypes)).Cast<MoveTypes>().ToList();
        }

        void Execute()
        {
            _result = _randomGenerator.GetMove();
        }

        [Test]
        public void Can_get_a_move()
        {
            Execute();

            _moveTypes.Any(m=> m == _result).Should().Be.True();
        }
    }
}
