using UnityEngine;

namespace Encore.Model.Stats
{
    public enum GameStats
    {
        Energy,
        Skill,
        Popularity,
        Fame
    }

    public class GameStatExtensions
    {
        public static string ToString(GameStats stat)
        {
            return stat switch
            {
                GameStats.Energy => "Energy",
                GameStats.Skill => "Skill",
                GameStats.Popularity => "Popularity",
                GameStats.Fame => "Fame",
                _ => stat.ToString()
            };
        }

        public static Color GetDefaultColour(GameStats stat)
        {
            return stat switch
            {
                GameStats.Energy => Color.green,
                GameStats.Skill => Color.orange,
                GameStats.Popularity => Color.blue,
                GameStats.Fame => Color.purple,
                _ => Color.black
            };
        }
    }
}