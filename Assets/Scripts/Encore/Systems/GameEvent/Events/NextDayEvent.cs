using Encore.Abstractions.Interfaces;
using Encore.Systems.Core;

namespace Encore.Systems.GameEvent.Events
{
    public sealed class NextDayEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.DayAdvanced;
        public override string EventName => "NextDay";
        public override string Description => "A new day begins.";

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            if (!RequirementsAreMet(state, stats)) return this;
            
            dayService.Advance();

            return this;
        }
    }
}