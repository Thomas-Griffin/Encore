using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Player.Actions
{
    public class Rest : PlayerAction
    {
        public override GameEventBase ToGameEvent()
        {
            return new RestEvent
            {
                Action = this
            };
        }
    }
}