namespace Encore.Model.Game
{
    public enum GameState
    {
        Playing,
        Lose,
        Win
    }
    
    public static class GameStateExtensions
    {
        public static string ToString(GameState state)
        {
            return state switch
            {
                GameState.Playing => "Playing",
                GameState.Lose => "Lose",
                GameState.Win => "Win",
                _ => "Playing"
            };
        }

        public static GameState FromString(string stateString)
        {
            return stateString.ToLower() switch
            {
                "playing" => GameState.Playing,
                "lose" => GameState.Lose,
                "win" => GameState.Win,
                _ => GameState.Playing
            };
        }
    }
}