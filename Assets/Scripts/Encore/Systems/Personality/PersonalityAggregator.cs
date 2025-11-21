using System.Collections.Generic;
using System.Linq;
using Encore.Model.BandMember;
using UnityEngine;

namespace Encore.Systems.Personality
{
    public static class PersonalityAggregator
    {
        private static PersonalityDeltas ModifiersForTrait(PersonalityTraits trait)
        {
            return PersonalityTraitBonusMap.For(trait);
        }

        public static PersonalityDeltas AggregateForBandMember(BandMember member)
        {
            PersonalityDeltas aggregateForMember = new();
            return member?.PersonalityTraits?.Aggregate(aggregateForMember,
                (current, trait) => current + ModifiersForTrait(trait)) ?? aggregateForMember;
        }

        public static float MemberPerformanceContribution(BandMember member, float baseSkill = 1.0f)
        {
            if (member == null) return 0f;
            PersonalityDeltas deltas = AggregateForBandMember(member);
            float value = baseSkill * (1.0f + deltas.PerformanceDelta);
            return Mathf.Clamp(value, 0f, 2f);
        }

        public static List<PersonalityTraits> NormaliseTraits(IEnumerable<PersonalityTraits> traits)
        {
            return PersonalityTraitNormaliser.Normalise(traits);
        }
    }
}