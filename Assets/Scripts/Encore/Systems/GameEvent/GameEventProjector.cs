using System.Collections.Generic;
using Encore.Model.Game;
using Encore.Systems.GameEvent.Events;

namespace Encore.Systems.GameEvent
{
    public static class GameEventProjector
    {
        public static GameInstance RebuildState(IEnumerable<GameEventBase> events, GameInstance gameInstance)
        {
            foreach (GameEventBase gameEvent in events)
            {
                gameInstance.ApplyEvent(gameEvent);
            }

            return gameInstance;
        }
    }
}