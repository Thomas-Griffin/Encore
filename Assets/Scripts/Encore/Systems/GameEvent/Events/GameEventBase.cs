using System;
using Encore.Abstractions.Interfaces;
using Encore.Model.Player;
using Encore.Systems.Core;
using JetBrains.Annotations;
using Encore.Systems.GameEvent;

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

        [CanBeNull] public PlayerAction Action { get; set; }
        [CanBeNull] public GameEventBase DelegateEvent { get; set; }

        public int ConsecutiveEventRepetitions { get; set; } = 1;

        public bool RequirementsAreMet(GameSession state,
            IStatService stats)
            => Requirements.AreMetBy(state, stats);

        public abstract GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService);

        public virtual StatDeltas PreviewDeltas(GameSession state, IStatService stats, IDayService dayService)
        {
            return Deltas ?? new StatDeltas();
        }
    }
}