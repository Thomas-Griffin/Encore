using System;
using System.Collections.Generic;
using Encore.Model.Game;
using Encore.Model.Stats;
using UnityEngine;

namespace Encore.Systems
{
    public class StatManager : MonoBehaviour
    {
        // Fame target based on difficulty
        public static int FameTarget = 100;

        // Main stats — use properties with eager initialization to avoid Unity serialization nulling them
        public GameStat Energy { get; private set; } = new(GameStats.Energy, Color.green, 100, 0, 100, 20, 30, 10, 10);
        public GameStat Skill { get; private set; } = new(GameStats.Skill, Color.orange, 0, 0, 10, 1, 1);
        public GameStat Popularity { get; private set; } = new(GameStats.Popularity, Color.blue, 0, 0, 50, 5, 10, 2);
        public GameStat Fame { get; private set; } = new(GameStats.Fame, Color.purple, 0, 0, FameTarget, 10);
    

        public void InitialiseStats(DifficultyLevel difficulty)
        {
            int initialEnergy = difficulty switch
            {
                DifficultyLevel.Easy => 100,
                DifficultyLevel.Medium => 80,
                DifficultyLevel.Hard => 60,
                _ => 100
            };
            Energy.InitialValue = initialEnergy;
            Energy.CurrentValue = initialEnergy;
            Skill.CurrentValue = 0;
            Popularity.CurrentValue = 0;
            Fame.CurrentValue = 0;
            FameTarget = difficulty switch
            {
                DifficultyLevel.Easy => 50,
                DifficultyLevel.Medium => 100,
                DifficultyLevel.Hard => 200,
                _ => 50
            };
        }

        public void IncreaseStat(GameStats stat)
        {
            GetStat(stat)?.Increase();
        }

        public void DecreaseStat(GameStats stat)
        {
            GetStat(stat)?.Decrease();
        }

        public void IncreaseStatBy(GameStats stat, int amount)
        {
            GetStat(stat)?.IncreaseBy(amount);
        }

        public void DecreaseStatBy(GameStats stat, int amount)
        {
            GetStat(stat)?.DecreaseBy(amount);
        }

        private GameStat GetStat(GameStats stat)
        {
            return stat switch
            {
                GameStats.Energy => Energy,
                GameStats.Skill => Skill,
                GameStats.Popularity => Popularity,
                GameStats.Fame => Fame,
                _ => null
            };
        }

        public GameStat[] GetStats()
        {
            // Return an array of non-null stats to protect callers from unexpected null fields
            var list = new List<GameStat>();
            if (Energy != null) list.Add(Energy);
            if (Skill != null) list.Add(Skill);
            if (Popularity != null) list.Add(Popularity);
            if (Fame != null) list.Add(Fame);
            return list.ToArray();
        }

        public bool LoseConditionMet()
        {
            return Energy != null && Energy.IsAtMinimum();
        }

        public bool WinConditionMet()
        {
            return Fame != null && Fame.IsAtMaximum();
        }

        public void ResetStats()
        {
            // Use GetStats() which returns a non-null array of non-null stats (or empty array)
            var stats = GetStats();
            if (stats == null || stats.Length == 0) return;

            foreach (var stat in stats)
            {
                if (stat == null) continue;
                try
                {
                    stat.Reset();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"StatManager.ResetStats: Failed to reset stat '{stat?.Stat.ToString() ?? "<null>"}' - {ex}");
                }
            }
        }

        // Calculate and add fame bonus based on skill and popularity
        public void AddFameBonus()
        {
            int skillFameWorth = 10; // 1 fame for every skillFameWorth points
            int popularityFameWorth = 15; // 1 fame for every popularityFameWorth points
            int skillBonus = (Skill != null) ? Skill.CurrentValue / skillFameWorth : 0;
            int popularityBonus = (Popularity != null) ? Popularity.CurrentValue / popularityFameWorth : 0;
            int fameBonus = skillBonus + popularityBonus;
            Fame?.IncreaseBy(fameBonus * FameScaleFactor());
        }

        // Determine fame scaling factor based on difficulty
        private static int FameScaleFactor()
        {
            return GameManager.CurrentDifficulty switch
            {
                DifficultyLevel.Easy => 1,
                DifficultyLevel.Medium => 2,
                DifficultyLevel.Hard => 3,
                _ => 1
            };
        }
    }
}