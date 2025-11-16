using System;
using Encore.Model.Game;
using JetBrains.Annotations;

namespace Encore.Systems.GameEvent.Events
{
    [Serializable]
    public abstract class GameEventBase
    {
        public Guid EventId { get; private set; } = Guid.NewGuid();

        public abstract EventTypes Type { get; set; }
        public abstract string EventName { get; }
        public abstract string Description { get; }
        public DateTime Timestamp { get; private set; } = DateTime.Now;

        public StatRequirements Requirements { get; set; } = new();
        public StatDeltas Deltas { get; set; } = new();
        [CanBeNull] public GameAction Action { get; set; }

        [CanBeNull] public GameEventBase DelegateEvent { get; set; }

        public int ConsecutiveEventRepetitions { get; set; } = 1;
        
        public bool RequirementsAreMet(GameInstance state)
        {
            return Requirements.AreMetBy(state.Stats);
        }
        public abstract GameEventBase Apply(GameInstance state);
    }
}