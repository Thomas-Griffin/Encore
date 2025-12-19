using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Player.Actions
{
    public class Gig : PlayerAction
    {
        public override GameEventBase ToGameEvent()
        {
            return new GigEvent
            {
                Action = this
            };
        }
    }
}