using System;
using System.Reflection;
using Encore.Model.Player;
using Encore.Systems.GameEvent;
using Encore.Systems.GameEvent.Events;

namespace Encore.Systems.Save
{
    [Serializable]
    public class EventSnapshot
    {
        public string eventType;
        public string eventName;
        public string description;
        public string timestamp;
        public StatDeltas deltas;
        public GameActionSnapshot action;
        public int consecutiveRepetitions;

        public static GameEventBase ToGameEvent(EventSnapshot snapshot)
        {
            if (snapshot == null) return null;
            GameEventBase gameEvent = snapshot.eventType switch
            {
                "RestEvent" => new RestEvent(),
                "PractiseEvent" => new PractiseEvent(),
                "GigEvent" => new GigEvent(),
                "FameBonusEvent" => new FameBonusEvent(),
                "NextDayEvent" => new NextDayEvent(),
                "StartGameEvent" => new StartGameEvent(),
                _ => null
            };

            if (gameEvent == null) return null;
            if (DateTime.TryParse(snapshot.timestamp, out DateTime ts))
            {
                PropertyInfo propertyInfo = gameEvent.GetType().GetProperty("Timestamp",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (propertyInfo != null)
                {
                    MethodInfo setMethod = propertyInfo.GetSetMethod(true);
                    setMethod?.Invoke(gameEvent, new object[] { ts });
                }
            }

            gameEvent.Deltas = snapshot.deltas ?? new StatDeltas();
            if (snapshot.action != null && !string.IsNullOrWhiteSpace(snapshot.action.actionType) &&
                Enum.TryParse(snapshot.action.actionType, out PlayerActions playerActions))
            {
                gameEvent.Action = GameActionSnapshot.ToGameAction(snapshot.action);
            }

            gameEvent.ConsecutiveEventRepetitions = snapshot.consecutiveRepetitions > 0
                ? snapshot.consecutiveRepetitions
                : 1;

            return gameEvent;
        }

        public static EventSnapshot FromGameEvent(GameEventBase gameEvent)
        {
            if (gameEvent == null) return null;
            return new EventSnapshot
            {
                eventType = gameEvent.GetType().Name,
                eventName = gameEvent.EventName,
                description = gameEvent.Description,
                timestamp = gameEvent.Timestamp.ToString("o"),
                deltas = gameEvent.Deltas ?? new StatDeltas(),
                action = GameActionSnapshot.FromGameAction(gameEvent.Action),
                consecutiveRepetitions = gameEvent.ConsecutiveEventRepetitions
            };
        }
    }
}