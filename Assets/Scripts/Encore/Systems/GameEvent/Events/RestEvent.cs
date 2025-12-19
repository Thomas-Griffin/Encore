using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Systems.Core;
using UnityEngine;
using Encore.Systems.GameEvent;

namespace Encore.Systems.GameEvent.Events
{
    public class RestEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Rest";

        public override string Description => "You took some time to rest and recover your energy.";

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            Deltas.energyDelta = CalculateEnergyDelta(state, stats);
            Deltas.skillDelta = CalculateSkillDelta(state, stats);
            Deltas.popularityDelta = CalculatePopularityDelta();
            Deltas.fameDelta = CalculateFameDelta();

            if (!RequirementsAreMet(state, stats)) return this;
            stats.ApplyDeltas(Deltas);
            return this;
        }

        public override StatDeltas PreviewDeltas(GameSession state, IStatService stats, IDayService dayService)
        {
            StatDeltas deltas = new()
            {
                energyDelta = CalculateEnergyDelta(state, stats),
                skillDelta = CalculateSkillDelta(state, stats),
                popularityDelta = CalculatePopularityDelta(),
                fameDelta = CalculateFameDelta()
            };
            return deltas;
        }

        private int CalculateEnergyDelta(GameSession state,
            IStatService stats)
        {
            if (state == null || stats == null) return 0;

            const float baseEnergy = 15f;
            const float growthFactor = 1.25f; // each consecutive rest increases gain by 25%
            int repetitions = Mathf.Max(1, ConsecutiveEventRepetitions);
            int calculatedEnergy = Mathf.RoundToInt(baseEnergy * Mathf.Pow(growthFactor, repetitions - 1));

            if (stats.Energy == null) return calculatedEnergy;
            int remaining = stats.Energy.MaxValue - stats.Energy.CurrentValue;
            calculatedEnergy = Mathf.Clamp(calculatedEnergy, 0, Mathf.Max(0, remaining));

            return calculatedEnergy;
        }

        private int CalculateSkillDelta(GameSession state,
            IStatService stats)
        {
            if (state == null || stats?.Skill == null) return 0;

            if (ConsecutiveEventRepetitions >= 2)
            {
                return -1 * ConsecutiveEventRepetitions + stats.Skill.CurrentValue / 10;
            }

            return 0;
        }

        private int CalculatePopularityDelta()
        {
            return -1 * ConsecutiveEventRepetitions;
        }

        private static int CalculateFameDelta()
        {
            return 0;
        }
    }
}