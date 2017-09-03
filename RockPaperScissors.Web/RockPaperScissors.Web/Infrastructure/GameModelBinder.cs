using RockPaperScissors.Web.Models;
using System.Web.Mvc;

namespace RockPaperScissors.Web.Infrastrucure
{
    public class GameModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Game game = (Game)controllerContext.HttpContext.Session["Game"];
            if (game == null)
            {
                game = Game.Start(Game.DefaultNumberOfPlayers, Game.DefaultNumberOfTries);
                controllerContext.HttpContext.Session["Game"] = game;
            }

            return game;
        }
    }
}