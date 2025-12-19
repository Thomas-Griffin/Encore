using System;
using System.Collections.Generic;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Core;

namespace Encore.Systems.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public string saveFileName;
        public string playState;
        public Difficulty difficulty;
        public int daysCurrent;
        public int daysTotal;

        public List<StatSnapshot> stats = new();
        public List<EventSnapshot> events = new();

        public static SaveData FromGame(
            GameSession session,
            IStatService statService,
            IDayService dayService,
            IEventStore eventStore)
        {
            SaveData saveData = new()
            {
                playState = PlayStateExtensions.ToString(session?.PlayState ?? PlayState.Playing),
                difficulty = session?.Difficulty ?? Difficulty.Easy,
                daysCurrent = dayService.CurrentDay,
                daysTotal = dayService.TotalDays
            };

            if (statService != null)
            {
                foreach (GameStat gameStat in statService.GetStats())
                {
                    if (gameStat == null) continue;
                    saveData.stats.Add(new StatSnapshot
                    {
                        statName = gameStat.Stat.ToString(),
                        currentValue = gameStat.CurrentValue,
                        initialValue = gameStat.InitialValue,
                        minValue = gameStat.MinValue,
                        maxValue = gameStat.MaxValue
                    });
                }
            }

            if (eventStore != null)
            {
                saveData.events.AddRange(eventStore.EventsAsSnapshots());
            }

            return saveData;
        }

        public GameSession ToGameSession()
        {
            return new GameSession
            {
                PlayState = PlayStateExtensions.FromString(playState ?? "Playing"),
                Difficulty = difficulty,
                LoseReasons = new List<LoseReasons>(),
                WinReasons = new List<WinReasons>()
            };
        }
    }
}