using Encore.Abstractions.Interfaces;
using Encore.Systems.Core;
using UnityEngine;
using Encore.Systems.GameEvent;

namespace Encore.Systems.GameEvent.Events
{
    public class PractiseEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Practise";

        public override string Description => "You spent time practising your skills.";

        public PractiseEvent()
        {
            Requirements = new StatRequirements
            {
                MinEnergy = 0,
                MinSkill = 0,
                MinPopularity = 0,
                MinFame = 0
            };
        }

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            Deltas.energyDelta = CalculateEnergyDelta();
            Deltas.skillDelta = CalculateSkillDelta();
            Deltas.popularityDelta = 0;
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
                skillDelta = CalculateSkillDelta(),
                popularityDelta = 0,
                fameDelta = 0
            };
            return deltas;
        }

        private int CalculateSkillDelta()
        {
            const float maximumGainAtBellCurvePeak = 3f;
            const float repetitionCountWherePracticeMostEffective = 3f;
            const float bellCurveWidth = 1.2f;

            float repetitions = Mathf.Max(1f, ConsecutiveEventRepetitions);

            float exponent =
                -((repetitions - repetitionCountWherePracticeMostEffective) *
                  (repetitions - repetitionCountWherePracticeMostEffective)) / (2f * bellCurveWidth * bellCurveWidth);
            float rawGain = maximumGainAtBellCurvePeak * Mathf.Exp(exponent);

            int gain = Mathf.Clamp(Mathf.RoundToInt(rawGain), 1, Mathf.RoundToInt(maximumGainAtBellCurvePeak));
            return gain;
        }

        private int CalculateEnergyDelta()
        {
            float reps = Mathf.Max(1f, ConsecutiveEventRepetitions);
            const int baseCost = 5;
            int extra = Mathf.RoundToInt(Mathf.Pow(reps - 1f, 1.5f) * 2f);
            return -(baseCost + extra);
        }
    }
}