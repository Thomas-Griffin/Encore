using System;
using Encore.Model.Game;
using UnityEngine;

namespace Encore.Systems.GameEvent.Events
{
    public class FameBonusEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.StatChanged;

        public override string EventName => "FameBonus";

        public override string Description => "You received a boost in fame!";

        private readonly DifficultyLevel _difficulty;

        public FameBonusEvent(DifficultyLevel difficulty = DifficultyLevel.Easy)
        {
            _difficulty = difficulty;
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
            Deltas.energyDelta = 0;
            Deltas.skillDelta = 0;
            Deltas.popularityDelta = 0;
            Deltas.fameDelta = CalculateFameDelta(state);
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }

        private int CalculateFameDelta(GameInstance state)
        {
            if (state == null || !state.Stats) return 0;

            int skill = state.Stats.Skill?.CurrentValue ?? 0;
            int popularity = state.Stats.Popularity?.CurrentValue ?? 0;
            int fameCurrent = state.Stats.Fame?.CurrentValue ?? 0;
            int fameMax = state.Stats.Fame?.MaxValue ?? 0;

            int denominator = fameMax - fameCurrent;
            if (denominator <= 0) return 0;

            long product = (long)skill * popularity * ConsecutiveEventRepetitions;
            long baseValue = product / denominator;

            float baseNormalized = fameMax > 0 ? baseValue / (float)fameMax : 0f;

            // Diminishing returns curve: early gains easier, late gains harder.
            float ratio = fameMax > 0 ? fameCurrent / (float)fameMax : 0f;
            float exponent = _difficulty switch
            {
                DifficultyLevel.Easy => 1.2f,
                DifficultyLevel.Medium => 1.6f,
                DifficultyLevel.Hard => 2.0f,
                _ => 1.2f
            };
            float scarcityFactor = 1f - Mathf.Pow(Mathf.Clamp01(ratio), exponent);

            float finalNormalized = baseNormalized * scarcityFactor;

            float rawCompound = fameCurrent * finalNormalized;
            int compoundDelta = Mathf.RoundToInt(rawCompound);

            int fallback = (int)Mathf.Clamp(baseValue * scarcityFactor, 0, fameMax - fameCurrent);
            int result = compoundDelta > 0 ? compoundDelta : fallback;

            // Apply a difficulty-tuned multiplier to make fame gains more significant
            float gainMultiplier = _difficulty switch
            {
                DifficultyLevel.Easy => 10f,
                DifficultyLevel.Medium => 7f,
                DifficultyLevel.Hard => 5f,
                _ => 10f
            };

            int amplified = Mathf.RoundToInt(result * gainMultiplier);
            int allowedMax = Math.Min(state.Stats.Fame!.MaximumIncrease, fameMax - fameCurrent);
            int allowedMin = Math.Min(state.Stats.Fame!.MinimumIncrease, Math.Max(0, allowedMax));
            result = Mathf.Clamp(amplified, allowedMin, Math.Max(0, allowedMax));

            return result;
        }
    }
}