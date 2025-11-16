using System;
using System.Collections.Generic;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using Encore.Systems.GameEvent.Events;
using UnityEngine;

namespace Encore.Model.Game
{
    [Serializable]
    public class GameInstance
    {
        public EventStore Events { get; set; }
        public GameState State { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public StatManager Stats { get; set; }
        public DayManager Days { get; set; }

        public string SaveFileName { get; set; }

        public List<LoseReasons> LoseReasons { get; set; } = new();
        public List<WinReasons> WinReasons { get; set; } = new();

        public GameInstance()
        {
            SaveFileName = "encore_autosave";
            Events = new EventStore(SaveFileName);
            State = GameState.Playing;
            Difficulty = DifficultyLevel.Easy;
            Stats = ScriptableObject.CreateInstance<StatManager>();
            Days = new DayManager(Difficulty);
        }

        public GameInstance(string saveFileName, DifficultyLevel difficulty)
        {
            SaveFileName = saveFileName;
            Events = new EventStore(SaveFileName);
            State = GameState.Playing;
            Difficulty = difficulty;
            Stats = ScriptableObject.CreateInstance<StatManager>();
            Days = new DayManager(Difficulty);
        }

        public GameEventBase ApplyEvent(GameEventBase gameEvent)
        {
            return CalculateEventRepetitions(gameEvent).Apply(this);
        }

        private GameEventBase CalculateEventRepetitions(GameEventBase gameEvent)
        {
            GameEventBase lastActionEvent = null;
            List<GameEventBase> all = Events.GetAllEvents();
            for (int i = all.Count - 1; i >= 0; i--)
            {
                if (all[i] == null || all[i].Action == null) continue;
                lastActionEvent = all[i];
                break;
            }

            if (lastActionEvent != null && gameEvent.Action != null &&
                lastActionEvent.Action!.Type == gameEvent.Action.Type)
            {
                gameEvent.ConsecutiveEventRepetitions = lastActionEvent.ConsecutiveEventRepetitions + 1;
            }
            else
            {
                gameEvent.ConsecutiveEventRepetitions = 1;
            }

            return gameEvent;
        }

        public GameAction GetLastAction()
        {
            return Events.GetLastAction();
        }
    }
}