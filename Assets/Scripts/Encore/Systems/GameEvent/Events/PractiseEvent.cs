using Encore.Model.Game;
using UnityEngine;

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

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = CalculateEnergyDelta();
            Deltas.skillDelta = CalculateSkillDelta();
            Deltas.popularityDelta = 0;
            Deltas.fameDelta = 0;
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }

        // Bell-curve skill gain: slow at first, peak around middle, then decline at high repetitions.
        private int CalculateSkillDelta()
        {
            const float maximumGainAtBellCurvePeak = 3f;
            const float repetitionCountWherePracticeMostEffective = 3f;
            const float bellCurveWidth = 1.2f;

            float repetitions = Mathf.Max(1f, ConsecutiveEventRepetitions);

            float exponent = -((repetitions - repetitionCountWherePracticeMostEffective) * (repetitions - repetitionCountWherePracticeMostEffective)) / (2f * bellCurveWidth * bellCurveWidth);
            float rawGain = maximumGainAtBellCurvePeak * Mathf.Exp(exponent);
            
            int gain = Mathf.Clamp(Mathf.RoundToInt(rawGain), 1, Mathf.RoundToInt(maximumGainAtBellCurvePeak));
            return gain;
        }

        // Energy cost grows with repetitions (non-linear) so prolonged practice gets harder to maintain.
        private int CalculateEnergyDelta()
        {
            float reps = Mathf.Max(1f, ConsecutiveEventRepetitions);
            const int baseCost = 5;
            // extra cost scales like (reps-1)^1.5 times a factor
            int extra = Mathf.RoundToInt(Mathf.Pow(reps - 1f, 1.5f) * 2f);
            return -(baseCost + extra);
        }
    }
}