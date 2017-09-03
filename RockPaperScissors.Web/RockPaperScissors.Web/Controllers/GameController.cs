using RockPaperScissors.Web.Domain;
using RockPaperScissors.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RockPaperScissors.Web.Controllers
{
    public class GameController : Controller
    {
        readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public GameController() : this(new GameService()) { }

        [HttpGet]
        public ActionResult Index(Game game)
        {
            _gameService.ResetGame(game);

            return View(game);
        }

        [HttpPost]
        public ActionResult Index(Game game, List<PlayerTypes> playerTypes)
        {
            if(game.NumberOfPlayers != playerTypes.Count)
            {
                throw new ArgumentException($"The number of players {game.NumberOfPlayers} is different from the number of types {playerTypes.Count}");
            }

            _gameService.SetupPlayers(game, playerTypes);

            return RedirectToAction(nameof(GameController.Play));
        }

        [HttpGet]
        public ActionResult Play(Game game)
        {
             _gameService.NextMove(game);
            return View(game);
        }

        [HttpPost]
        public ActionResult Play(Game game, List<MoveTypes> moveTypes)
        {
            _gameService.SetWinnerForLastTry(game, moveTypes);
            _gameService.TryToSetGameWinner(game);

            if(!game.HasFinished)
                return RedirectToAction(nameof(GameController.Play));

            return RedirectToAction(nameof(GameController.End));
        }

        [HttpGet]
        public ActionResult End(Game game)
        {
            return View(game);
        }
    }
}