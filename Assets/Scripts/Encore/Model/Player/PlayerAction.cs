using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Player
{
    public abstract class PlayerAction
    {
        public abstract GameEventBase ToGameEvent();

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}