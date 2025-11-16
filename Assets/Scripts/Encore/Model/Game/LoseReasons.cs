namespace Encore.Model.Game
{
    public enum LoseReasons
    {
        RanOutOfTime,
        EnergyDepleted
    }

    public class LoseReasonExtensions
    {
        public static string ToDescription(LoseReasons reason)
        {
            return reason switch
            {
                LoseReasons.RanOutOfTime => "You used up all your tour days without achieving the fame target.",
                LoseReasons.EnergyDepleted => "Your energy has dropped to zero.",
                _ => "Unknown reason."
            };
        }
    }
}