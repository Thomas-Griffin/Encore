using System;
using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    [Serializable]
    public class StartGameEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.GameStarted;
        public override string EventName => "Game Started";
        public override string Description => "A new game session has started.";

        public override GameEventBase Apply(GameInstance state)
        {
            return this;
        }
    }
}