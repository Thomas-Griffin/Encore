using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using Encore.Systems.GameEvent.Events;
using UnityEngine;

namespace Encore.Systems.Save
{
    [Serializable]
    public class GameInstanceSnapshot
    {
        public string saveFileName;
        public string state;
        public DifficultyLevel difficulty;
        public int daysCurrent;
        public int daysTotal;
        public List<StatSnapshot> stats = new();
        public List<EventSnapshot> events = new();

        public static GameInstanceSnapshot FromGameInstance(GameInstance gameInstance)
        {
            GameInstanceSnapshot gameInstanceSnapshot = new()
            {
                saveFileName = gameInstance?.SaveFileName ?? "encore_autosave",
                state = GameStateExtensions.ToString(gameInstance?.State ?? GameState.Playing),
                difficulty = gameInstance?.Difficulty ?? DifficultyLevel.Easy,
                daysCurrent = gameInstance?.Days?.CurrentDay ?? 1,
                daysTotal = gameInstance?.Days?.TotalDays ?? 50
            };

            if (gameInstance?.Stats)
            {
                GameStat[] gameStats = gameInstance.Stats.GetStats();
                foreach (GameStat gameStat in gameStats)
                {
                    if (gameStat == null) continue;
                    gameInstanceSnapshot.stats.Add(new StatSnapshot
                    {
                        statName = gameStat.Stat.ToString(),
                        currentValue = gameStat.CurrentValue,
                        initialValue = gameStat.InitialValue,
                        minValue = gameStat.MinValue,
                        maxValue = gameStat.MaxValue
                    });
                }
            }

            // Capture events
            if (gameInstance?.Events == null) return gameInstanceSnapshot;
            List<GameEventBase> gameEvents = gameInstance.Events.GetAllEvents();
            foreach (EventSnapshot eventSnapshot in gameEvents.Select(EventSnapshot.FromGameEvent)
                         .Where(es => es != null))
            {
                gameInstanceSnapshot.events.Add(eventSnapshot);
            }

            return gameInstanceSnapshot;
        }

        public GameInstance ToGameInstance()
        {
            GameInstance gameInstance = new(saveFileName, difficulty)
            {
                State = GameStateExtensions.FromString(state ?? "Playing")
            };
            StatManager statManager = ScriptableObject.CreateInstance<StatManager>();
            statManager.InitialiseStats(difficulty);

            GameStat[] statsArr = statManager.GetStats();
            foreach (StatSnapshot statSnapshot in stats.Where(snapshot => snapshot != null))
            {
                if (!Enum.TryParse(statSnapshot.statName, out GameStats statEnum)) continue;
                foreach (GameStat gameStat in statsArr)
                {
                    if (gameStat == null) continue;
                    if (gameStat.Stat != statEnum) continue;
                    gameStat.InitialValue = statSnapshot.initialValue;
                    gameStat.CurrentValue = statSnapshot.currentValue;
                    gameStat.ClampValue();
                    break;
                }
            }

            gameInstance.Stats = statManager;

            gameInstance.Days = new DayManager(difficulty);
            for (int i = 1; i < daysCurrent && i < gameInstance.Days.TotalDays; i++)
            {
                gameInstance.Days.Advance();
            }

            if (events is not { Count: > 0 }) return gameInstance;

            string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");

            if (!Directory.Exists(saveDirectory)) Directory.CreateDirectory(saveDirectory);

            string path = Path.Combine(saveDirectory, saveFileName + ".jsonl");

            using (StreamWriter streamWriter = new(path, true))
            {
                foreach (EventSnapshot eventSnapshot in events.Where(snapshot => snapshot != null))
                {
                    streamWriter.WriteLine(JsonUtility.ToJson(eventSnapshot));
                }
            }

            gameInstance.Events = new EventStore(saveFileName);

            return gameInstance;
        }
    }
}