using System.Linq;
using Encore.Model.Band;
using Encore.Model.BandMember;
using UnityEngine;

namespace Encore.Systems.Personality
{
    public static class PersonalitySimulator
    {
        public static float EvaluateGigUsingPersonalityTraits(Band band)
        {
            if (band?.Members == null || band.Members.Count == 0) return 0f;

            float averageContribution = band.Members
                .Select(bandMember => PersonalityAggregator.MemberPerformanceContribution(bandMember, 0f))
                .Average();

            float totalCreativity = band.Members
                .Select(bandMember => PersonalityAggregator.AggregateForBandMember(bandMember).CreativityDelta)
                .Sum();

            float quality = Mathf.Clamp((averageContribution * 50f) + (totalCreativity * 30f), 0f, 100f);
            return quality;
        }

        private static float MoraleToFloat(BandMemberMoraleLevels morale)
        {
            return morale switch
            {
                BandMemberMoraleLevels.VeryLow => 0.0f,
                BandMemberMoraleLevels.Low => 0.25f,
                BandMemberMoraleLevels.Medium => 0.5f,
                BandMemberMoraleLevels.High => 0.75f,
                BandMemberMoraleLevels.VeryHigh => 1.0f,
                _ => 0.5f
            };
        }

        private static BandMemberMoraleLevels FloatToMorale(float value)
        {
            value = Mathf.Clamp01(value);
            return value switch
            {
                <= 0f => BandMemberMoraleLevels.VeryLow,
                <= 0.25f => BandMemberMoraleLevels.Low,
                <= 0.5f => BandMemberMoraleLevels.Medium,
                <= 0.75f => BandMemberMoraleLevels.High,
                _ => BandMemberMoraleLevels.VeryHigh
            };
        }

        public static void ApplyPostGigMoraleToBandMembers(Band band, float baseMoraleChange)
        {
            if (band?.Members == null) return;

            for (int i = 0; i < band.Members.Count; i++)
            {
                BandMember bandMember = band.Members[i];
                if (bandMember == null) continue;

                PersonalityDeltas personalityDeltas = PersonalityAggregator.AggregateForBandMember(bandMember);

                float tierSize = 0.25f;
                float adjustedModifier = 1f - Mathf.Clamp01(personalityDeltas.MoraleDelta);
                float finalChange = baseMoraleChange * tierSize * adjustedModifier;

                float currentMoraleValue = MoraleToFloat(bandMember.Morale);
                float updatedMoraleValue = Mathf.Clamp01(currentMoraleValue + finalChange);
                band.Members[i] = bandMember with { Morale = FloatToMorale(updatedMoraleValue) };
            }
        }
    }
}