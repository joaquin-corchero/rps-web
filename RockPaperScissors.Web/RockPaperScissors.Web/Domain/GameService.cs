using RockPaperScissors.Web.Domain.Factories;
using RockPaperScissors.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Web.Domain
{
    public interface IGameService
    {
        void NextMove(Game game);
        void SetupPlayers(Game game, List<PlayerTypes> playerTypes);
        void SetWinnerForLastTry(Game game, List<MoveTypes> moves);
        void TryToSetGameWinner(Game game);
        void ResetGame(Game game);
    }

    public class GameService : IGameService
    {
        readonly IPlayerFactory _playerFactory;
        readonly INextMoveFactory _movesFactory;
        readonly IMoveReferee _handWinnerCalculator;
        readonly IGameReferee _referee;

        public GameService(
            IPlayerFactory playerFactory, 
            INextMoveFactory movesFactory,
            IMoveReferee handWinnerCalculator,
            IGameReferee referee
            )
        {
            _playerFactory = playerFactory;
            _movesFactory = movesFactory;
            _handWinnerCalculator = handWinnerCalculator;
            _referee = referee;
        }

        public GameService() : this(new PlayerFactory(), new NextMoveFactory(), new TwoPlayerMoveReferee(), new GameReferee()) { }

        public void SetupPlayers(Game game, List<PlayerTypes> playerTypes)
        {
            if (game.NumberOfPlayers != playerTypes.Count)
                throw new ArgumentException($"The number of players {game.NumberOfPlayers} is different from the number of types {playerTypes.Count}");

            playerTypes.ForEach(t => game.Players.Add(_playerFactory.Generate(t)));
        }

        public void NextMove(Game game)
        {
            if (NumberOfTriesReached(game))
                return;

            if (NoWinnerDecidedForLastGame(game))
                return;

            List<MoveTypes> moves = new List<MoveTypes>();
            game.Players.ForEach(player => moves.Add(_movesFactory.Generate(player)));

            game.Tries.Add(Move.Generate(moves));
        }

        bool NumberOfTriesReached(Game game)
        {
            return game.Tries.Count == game.NumberOfTries;
        }

        bool NoWinnerDecidedForLastGame(Game game)
        {
            return game.Tries.Count > 0 && !game.Tries.LastOrDefault().WinnerIndex.HasValue;
        }

        public void SetWinnerForLastTry(Game game, List<MoveTypes> moves)
        {
            Move lastTry = GetLastTryWithUpdatedMoves(game, moves);
            var winnerIndex = _handWinnerCalculator.GetWinnerIndex(lastTry.MoveTypes);
            lastTry.SetWinner(winnerIndex);
        }

        Move GetLastTryWithUpdatedMoves(Game game, List<MoveTypes> moves)
        {
            var lastTry = game.Tries.LastOrDefault();
            lastTry.ResetMoves(moves);
            return lastTry;
        }

        public void TryToSetGameWinner(Game game)
        {
            var winnerIndex = _referee.GetWinner(game);

            if (winnerIndex.HasValue)
                game.SetWinner(winnerIndex);
        }

        public void ResetGame(Game game)
        {
            game.Players.Clear();
            game.Tries.Clear();
            game.SetWinner(null);
        }
    }
}
