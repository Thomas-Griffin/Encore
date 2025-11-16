using System;
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
            GameEventBase lastEvent = Events.GetLastEvent();
            if (lastEvent is { Action: not null } && gameEvent.Action != null &&
                lastEvent.Action.Type == gameEvent.Action.Type)
            {
                gameEvent.ConsecutiveEventRepetitions = lastEvent.ConsecutiveEventRepetitions + 1;
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