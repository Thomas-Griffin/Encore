namespace Encore.Model.Game
{
    public enum WinReasons
    {
        AchievedFameTarget,
    }


    public static class WinReasonExtensions
    {
        public static string ToDescription(WinReasons reason)
        {
            return reason switch
            {
                WinReasons.AchievedFameTarget => "You have reached the required fame level to win the game.",
                _ => "Unknown reason."
            };
        }
    }
}