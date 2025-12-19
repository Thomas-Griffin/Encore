using System;
using System.Linq;
using Encore.Abstractions.Interfaces;
using Encore.Systems.Core;
using Encore.Systems.Configurations;
using UnityEngine;
using Encore.Model.Stats;

namespace Encore.Systems.Save
{
    [Serializable]
    public class SavedGame
    {
        public SaveData saveData;

        public SavedGame(
            GameSession gameSession,
            IStatService stats,
            IDayService dayService,
            IEventStore eventStore,
            DateTime saveTimeStamp)
        {
            saveData = gameSession == null
                ? null
                : SaveData.FromGame(gameSession, stats, dayService, eventStore);
        }

        public GameSession ToGame()
        {
            return saveData?.ToGameSession();
        }


        public IStatService GetStats()
        {
            if (saveData == null) return null;

            StatsService statsService = new(ScriptableObject.CreateInstance<StatsConfig>());
            statsService.InitialiseStats(saveData.difficulty);

            GameStat[] statsArr = statsService.GetStats();
            if (statsArr == null || statsArr.Length == 0) return statsService;

            foreach (StatSnapshot saveDataStat in saveData.stats.Where(saveDataStat => saveDataStat != null))
            {
                if (!Enum.TryParse(saveDataStat.statName, out GameStats statEnum)) continue;

                foreach (GameStat gameStat in statsArr)
                {
                    if (gameStat == null) continue;
                    if (gameStat.Stat != statEnum) continue;

                    gameStat.InitialValue = gameStat.ClampedValue(saveDataStat.initialValue);
                    gameStat.CurrentValue = gameStat.ClampedValue(saveDataStat.currentValue);
                    break;
                }
            }

            return statsService;
        }
    }
}