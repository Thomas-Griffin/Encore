using System;
using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    public class GigEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Gig";

        public override string Description =>
            "You performed a gig! It was exhausting but your popularity has increased.";


        public GigEvent(DifficultyLevel difficulty = DifficultyLevel.Easy)
        {
            Requirements = new StatRequirements
            {
                MinSkill = difficulty switch
                {
                    DifficultyLevel.Easy => 3,
                    DifficultyLevel.Medium => 4,
                    DifficultyLevel.Hard => 5,
                    _ => 3
                },
            };
            DelegateEvent = new FameBonusEvent(difficulty);
        }

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = CalculateEnergyDelta();
            Deltas.skillDelta = 0;
            Deltas.popularityDelta = CalculatePopularityDelta();
            Deltas.fameDelta = 0;
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }


        private int CalculateEnergyDelta()
        {
            return -10 * ConsecutiveEventRepetitions;
        }

        private int CalculatePopularityDelta()
        {
            return 10 * ConsecutiveEventRepetitions;
        }
    }
}