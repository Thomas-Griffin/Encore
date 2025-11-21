using Encore.Model.Game;
using UnityEngine;

namespace Encore.Systems.GameEvent.Events
{
    public class RestEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Rest";

        public override string Description => "You took some time to rest and recover your energy.";

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = CalculateEnergyDelta(state);
            Deltas.skillDelta = CalculateSkillDelta(state);
            Deltas.popularityDelta = CalculatePopularityDelta();
            Deltas.fameDelta = CalculateFameDelta();

            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }

        private int CalculateEnergyDelta(GameInstance state)
        {
            if (state == null || !state.Stats) return 0;

            const float baseEnergy = 15f;
            const float growthFactor = 1.25f; // each consecutive rest increases gain by 25%
            int repetitions = Mathf.Max(1, ConsecutiveEventRepetitions);
            int calculatedEnergy = Mathf.RoundToInt(baseEnergy * Mathf.Pow(growthFactor, repetitions - 1));

            if (state.Stats.Energy == null) return calculatedEnergy;
            int remaining = state.Stats.Energy.MaxValue - state.Stats.Energy.CurrentValue;
            calculatedEnergy = Mathf.Clamp(calculatedEnergy, 0, Mathf.Max(0, remaining));

            return calculatedEnergy;
        }

        private int CalculateSkillDelta(GameInstance state)
        {
            if (state == null || !state.Stats || state.Stats.Skill == null) return 0;

            if (ConsecutiveEventRepetitions >= 2)
            {
                return -1 * ConsecutiveEventRepetitions + state.Stats.Skill.CurrentValue / 10;
            }

            return 0;
        }

        private int CalculatePopularityDelta()
        {
            return -1 * ConsecutiveEventRepetitions;
        }

        private int CalculateFameDelta()
        {
            return 0;
        }
    }
}