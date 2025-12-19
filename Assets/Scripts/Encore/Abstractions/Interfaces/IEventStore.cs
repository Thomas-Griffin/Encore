using System.Collections.Generic;
using Encore.Model.Player;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;

namespace Encore.Abstractions.Interfaces
{
    public interface IEventStore
    {
        int Count { get; }
        IEnumerable<GameEventBase> Events { get; }
        void Append(GameEventBase gameEvent);
        GameEventBase GetLastEvent();
        GameEventBase GetLastActionEvent();
        PlayerAction GetLastAction();
        void Clear();
        IEnumerable<EventSnapshot> EventsAsSnapshots();
    }
}