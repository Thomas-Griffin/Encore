using System;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;

namespace Encore.Systems.GameEvent.Events
{
    public class GigEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Gig";

        public override string Description =>
            "You performed a gig! It was exhausting but your popularity has increased.";


        public GigEvent(Difficulty difficulty = Difficulty.Easy)
        {
            Requirements = new StatRequirements
            {
                MinSkill = difficulty switch
                {
                    Difficulty.Easy => 3,
                    Difficulty.Medium => 4,
                    Difficulty.Hard => 5,
                    _ => 3
                },
            };
            DelegateEvent = new FameBonusEvent(difficulty);
        }

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            Deltas.energyDelta = CalculateEnergyDelta();
            Deltas.skillDelta = 0;
            Deltas.popularityDelta = CalculatePopularityDelta();
            Deltas.fameDelta = 0;
            if (!RequirementsAreMet(state, stats)) return this;
            stats.ApplyDeltas(Deltas);
            return this;
        }

        public override StatDeltas PreviewDeltas(GameSession state, IStatService stats, IDayService dayService)
        {
            StatDeltas deltas = new()
            {
                energyDelta = CalculateEnergyDelta(),
                skillDelta = 0,
                popularityDelta = CalculatePopularityDelta(),
                fameDelta = 0
            };
            return deltas;
        }

        private int CalculateEnergyDelta()
        {
            return Math.Clamp(-10 - (ConsecutiveEventRepetitions - 1), -30, -5);
        }

        private int CalculatePopularityDelta()
        {
            return Math.Clamp(5 * ConsecutiveEventRepetitions, 5, 30);
        }
    }
}