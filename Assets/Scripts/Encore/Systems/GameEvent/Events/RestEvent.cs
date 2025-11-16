using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    public class RestEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Rest";

        public override string Description => "You took some time to rest and recover your energy.";

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = 15 * ConsecutiveEventRepetitions;
            Deltas.skillDelta = (-1 * ConsecutiveEventRepetitions) + state.Stats.Skill.CurrentValue / 10;
            Deltas.popularityDelta = -1 * ConsecutiveEventRepetitions;
            Deltas.fameDelta = 0;
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }
    }
}