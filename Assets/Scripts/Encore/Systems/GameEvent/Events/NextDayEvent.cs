using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    public class NextDayEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.DayAdvanced;

        public override string EventName => "NextDay";

        public override string Description => "A new day begins.";
        public override GameEventBase Apply(GameInstance state)
        {
            if (!RequirementsAreMet(state)) return this;
            state.Days.Advance();
            return this;
        }
    }
}