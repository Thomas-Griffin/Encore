using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Player.Actions
{
    public class Practice : PlayerAction
    {
        public override GameEventBase ToGameEvent()
        {
            return new PractiseEvent
            {
                Action = this
            };
        }
    }
}