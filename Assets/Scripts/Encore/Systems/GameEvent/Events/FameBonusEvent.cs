using System;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Systems.Core;
using Encore.Systems.Save;
using UnityEngine;

namespace Encore.Systems.GameEvent.Events
{
    public class FameBonusEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.StatChanged;

        public override string EventName => "FameBonus";

        public override string Description => "You received a boost in fame!";

        private readonly Difficulty _difficulty;

        public FameBonusEvent(Difficulty difficulty = Difficulty.Easy)
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

        public override GameEventBase Apply(GameSession state,
            IStatService stats,
            IDayService dayService)
        {
            Deltas.energyDelta = 0;
            Deltas.skillDelta = 0;
            Deltas.popularityDelta = 0;
            Deltas.fameDelta = CalculateFameDelta(state, stats);
            if (!RequirementsAreMet(state, stats)) return this;
            stats.ApplyDeltas(Deltas);
            return this;
        }

        

        private int CalculateFameDelta(GameSession state,
            IStatService stats)
        {
            if (state == null || stats == null) return 0;

            int skill = stats.Skill?.CurrentValue ?? 0;
            int popularity = stats.Popularity?.CurrentValue ?? 0;
            int fameCurrent = stats.Fame?.CurrentValue ?? 0;
            int fameMax = stats.Fame?.MaxValue ?? 0;

            int denominator = fameMax - fameCurrent;
            if (denominator <= 0) return 0;

            long product = (long)skill * popularity * ConsecutiveEventRepetitions;
            long baseValue = product / denominator;

            float baseNormalized = fameMax > 0 ? baseValue / (float)fameMax : 0f;

            float ratio = fameMax > 0 ? fameCurrent / (float)fameMax : 0f;
            float exponent = _difficulty switch
            {
                Difficulty.Easy => 1.2f,
                Difficulty.Medium => 1.6f,
                Difficulty.Hard => 2.0f,
                _ => 1.2f
            };
            float scarcityFactor = 1f - Mathf.Pow(Mathf.Clamp01(ratio), exponent);

            float finalNormalized = baseNormalized * scarcityFactor;

            float rawCompound = fameCurrent * finalNormalized;
            int compoundDelta = Mathf.RoundToInt(rawCompound);

            int fallback = (int)Mathf.Clamp(baseValue * scarcityFactor, 0, fameMax - fameCurrent);
            int result = compoundDelta > 0 ? compoundDelta : fallback;


            float gainMultiplier = _difficulty switch
            {
                Difficulty.Easy => 10f,
                Difficulty.Medium => 7f,
                Difficulty.Hard => 5f,
                _ => 10f
            };

            int amplified = Mathf.RoundToInt(result * gainMultiplier);
            int allowedMax = Math.Min(stats.Fame!.MaximumIncrease, fameMax - fameCurrent);
            int allowedMin = Math.Min(stats.Fame!.MinimumIncrease, Math.Max(0, allowedMax));
            result = Mathf.Clamp(amplified, allowedMin, Math.Max(0, allowedMax));

            return result;
        }
    }
}