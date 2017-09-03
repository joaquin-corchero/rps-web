using RockPaperScissors.Web.Domain;
using System;
using System.Linq;

namespace RockPaperScissors.Web.Models.Domain.MoveGenerators
{
    public interface IRandomGenerator
    {
        MoveTypes GetMove();
    }

    public class RandomGenerator : IRandomGenerator
    {
        static readonly Random _random = new Random();
        static readonly object _syncLock = new object();
        private static int RandomNumber(int min, int max)
        {
            lock (_syncLock)
            { // synchronize
                return _random.Next(min, max);
            }
        }

        public MoveTypes GetMove()
        {
            var moves = Enum.GetValues(typeof(MoveTypes)).Cast<int>();

            var result = RandomGenerator.RandomNumber(moves.Min(), moves.Max());

            return (MoveTypes)result;
        }
    }
}