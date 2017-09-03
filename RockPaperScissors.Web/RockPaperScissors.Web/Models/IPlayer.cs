using RockPaperScissors.Web.Domain;

namespace RockPaperScissors.Web.Models
{
    public interface IPlayer{
        PlayerTypes Playertype { get; }
    }
}