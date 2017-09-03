namespace RockPaperScissors.Web.Domain
{
    public enum PlayerTypes
    {
        Human = 0,
        Random = 1,
        Tactical = 2
    }

    public enum MoveTypes
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }

    public enum Results
    {
        Draw = 0,
        Won = 1,
        Lost = 2
    }
}