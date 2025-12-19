using System;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Systems.Core;

namespace Encore.Systems.GameEvent.Events
{
    [Serializable]
    public class StartGameEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.GameStarted;
        public override string EventName => "Game Started";
        public override string Description => "A new game session has started.";

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            return this;
        }
    }
}