using System;
using Encore.Model.Stats;

namespace Encore.Systems.Save
{
    [Serializable]
    public class StatSnapshot
    {
        public string statName;
        public int currentValue;
        public int initialValue;
        public int minValue;
        public int maxValue;
        public int minimumIncrease;
        public int maximumIncrease;


        public static StatSnapshot FromGameStat(GameStat gameStat)
        {
            return new StatSnapshot
            {
                statName = GameStatExtensions.ToString(gameStat.Stat),
                currentValue = gameStat.CurrentValue,
                initialValue = gameStat.InitialValue,
                minValue = gameStat.MinValue,
                maxValue = gameStat.MaxValue,
                minimumIncrease = gameStat.MinimumIncrease,
                maximumIncrease = gameStat.MaximumIncrease
            };
        }

        public static GameStat ToGameStat(StatSnapshot snapshot)
        {
            if (snapshot == null) return null;

            GameStats statType = Enum.TryParse(snapshot.statName, out GameStats parsedStat)
                ? parsedStat
                : throw new ArgumentException($"Invalid stat name: {snapshot.statName}");

            return new GameStat(
                stat: statType,
                colour: GameStatExtensions.GetDefaultColour(statType),
                initialValue: snapshot.initialValue,
                minValue: snapshot.minValue,
                maxValue: snapshot.maxValue,
                minimumIncrease: snapshot.minimumIncrease,
                maximumIncrease: snapshot.maximumIncrease
            );
        }
    }
}