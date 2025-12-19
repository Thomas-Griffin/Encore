using System.Collections.Generic;
using System.Linq;
using Encore.Abstractions.Interfaces;
using Encore.Model.Player;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;

namespace Encore.Systems.GameEvent
{
    public sealed class EventStore : IEventStore
    {
        private readonly List<GameEventBase> _events = new();

        public int Count => _events.Count;
        public IEnumerable<GameEventBase> Events => _events;

        public void Append(GameEventBase gameEvent)
        {
            _events.Add(gameEvent);
        }

        public GameEventBase GetLastEvent() => _events.Count > 0 ? _events[^1] : null;

        public GameEventBase GetLastActionEvent()
        {
            for (int i = _events.Count - 1; i >= 0; i--)
            {
                GameEventBase gameEventBase = _events[i];
                if (gameEventBase?.Action != null) return gameEventBase;
            }

            return null;
        }

        public PlayerAction GetLastAction() => GetLastActionEvent()?.Action;

        public void Clear() => _events.Clear();

        public IEnumerable<EventSnapshot> EventsAsSnapshots()
        {
            return _events.Select(EventSnapshot.FromGameEvent);
        }
    }
}