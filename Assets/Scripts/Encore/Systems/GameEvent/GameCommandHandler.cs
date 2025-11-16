using System;
using Encore.Model.Game;
using Encore.Systems.GameEvent.Events;

namespace Encore.Systems.GameEvent
{
    public class GameCommandHandler
    {
        private readonly EventStore _store;

        public GameCommandHandler(EventStore store) => _store = store;

        public void PerformAction(GameActions action)
        {
            GameEventBase gameEvent = action switch
            {
                GameActions.Rest => new RestEvent(),
                GameActions.Gig => new GigEvent(),
                GameActions.Practice => new PractiseEvent(),
                _ => throw new ArgumentOutOfRangeException()
            };

            _store.Append(gameEvent);
        }
    }
}