namespace Encore.Model.Game
{
    public enum PlayState
    {
        Playing,
        Lose,
        Win
    }
    
    public static class PlayStateExtensions
    {
        public static string ToString(PlayState state)
        {
            return state switch
            {
                PlayState.Playing => "Playing",
                PlayState.Lose => "Lose",
                PlayState.Win => "Win",
                _ => "Playing"
            };
        }

        public static PlayState FromString(string stateString)
        {
            return stateString.ToLower() switch
            {
                "playing" => PlayState.Playing,
                "lose" => PlayState.Lose,
                "win" => PlayState.Win,
                _ => PlayState.Playing
            };
        }
    }
}