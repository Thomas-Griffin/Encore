using Encore.Model.BandMember;

namespace Encore.Systems.Personality
{
    // Mapping PersonalityTraits enum to PersonalityModifiers.
    public static class PersonalityTraitBonusMap
    {
        public static PersonalityDeltas For(PersonalityTraits trait)
        {
            PersonalityEffect effect = trait switch
            {
                // Positive personality traits
                PersonalityTraits.Charismatic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.PositiveHigh,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveLow
                },
                PersonalityTraits.Diligent => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.PositiveLow,
                    Morale = PersonalityEffectLevels.PositiveMedium,
                    ConflictChance = PersonalityEffectLevels.NegativeHigh
                },
                PersonalityTraits.Creative => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveMax,
                    Performance = PersonalityEffectLevels.PositiveHigh,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveMedium
                },
                PersonalityTraits.Resilient => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.PositiveMax,
                    ConflictChance = PersonalityEffectLevels.None
                },
                PersonalityTraits.Empathetic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.PositiveMedium,
                    ConflictChance = PersonalityEffectLevels.NegativeMax
                },
                PersonalityTraits.Ambitious => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMedium,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.PositiveMax,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveMedium
                },
                PersonalityTraits.Adaptable => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveLow,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.PositiveMedium,
                    Morale = PersonalityEffectLevels.PositiveMedium,
                    ConflictChance = PersonalityEffectLevels.NegativeMax
                },
                PersonalityTraits.Confident => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeLow,
                    Creativity = PersonalityEffectLevels.PositiveMedium,
                    Performance = PersonalityEffectLevels.PositiveMax,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Patient => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.None,
                    ConflictChance = PersonalityEffectLevels.NegativeMax
                },
                PersonalityTraits.Optimistic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMedium,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.PositiveLow,
                    Morale = PersonalityEffectLevels.PositiveMax,
                    ConflictChance = PersonalityEffectLevels.NegativeLow
                },
                PersonalityTraits.Humorous => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.PositiveMax,
                    ConflictChance = PersonalityEffectLevels.NegativeMedium
                },
                PersonalityTraits.Analytical => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.PositiveMedium,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveLow
                },
                PersonalityTraits.Loyal => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.None,
                    ConflictChance = PersonalityEffectLevels.NegativeHigh
                },
                PersonalityTraits.Resourceful => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveHigh,
                    Performance = PersonalityEffectLevels.PositiveMedium,
                    Morale = PersonalityEffectLevels.PositiveMedium,
                    ConflictChance = PersonalityEffectLevels.None
                },
                PersonalityTraits.Passionate => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveMedium,
                    Creativity = PersonalityEffectLevels.PositiveMax,
                    Performance = PersonalityEffectLevels.PositiveHigh,
                    Morale = PersonalityEffectLevels.PositiveMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Observant => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveMedium,
                    Performance = PersonalityEffectLevels.PositiveLow,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.None
                },
                PersonalityTraits.Strategic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveHigh,
                    Creativity = PersonalityEffectLevels.PositiveMedium,
                    Performance = PersonalityEffectLevels.PositiveMedium,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.NegativeMedium
                },
                PersonalityTraits.Supportive => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveHigh,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.PositiveLow,
                    Morale = PersonalityEffectLevels.PositiveMax,
                    ConflictChance = PersonalityEffectLevels.NegativeMax
                },
                PersonalityTraits.Innovative => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.PositiveMax,
                    Performance = PersonalityEffectLevels.PositiveHigh,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Disciplined => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveHigh,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.PositiveMax,
                    Morale = PersonalityEffectLevels.PositiveLow,
                    ConflictChance = PersonalityEffectLevels.NegativeLow
                },
                PersonalityTraits.Collaborative => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.PositiveHigh,
                    Creativity = PersonalityEffectLevels.PositiveLow,
                    Performance = PersonalityEffectLevels.PositiveMedium,
                    Morale = PersonalityEffectLevels.PositiveMax,
                    ConflictChance = PersonalityEffectLevels.NegativeHigh
                },

                // Negative personality traits
                PersonalityTraits.Lazy => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.NegativeLow,
                    Performance = PersonalityEffectLevels.NegativeMedium,
                    Morale = PersonalityEffectLevels.NegativeLow,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Arrogant => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeHigh,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Impulsive => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMedium,
                    Creativity = PersonalityEffectLevels.NegativeLow,
                    Performance = PersonalityEffectLevels.NegativeLow,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Stubborn => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.None,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Pessimistic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeHigh,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeMedium,
                    Morale = PersonalityEffectLevels.NegativeMax,
                    ConflictChance = PersonalityEffectLevels.PositiveMedium
                },
                PersonalityTraits.Indecisive => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeLow,
                    Creativity = PersonalityEffectLevels.NegativeMedium,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.None
                },
                PersonalityTraits.Aloof => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeLow,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Cynical => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeMax,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Reckless => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeLow,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Selfish => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeHigh,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeMedium,
                    Morale = PersonalityEffectLevels.NegativeMax,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Inflexible => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.NegativeMedium,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Distrustful => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeLow,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Complacent => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeLow,
                    Creativity = PersonalityEffectLevels.NegativeMedium,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeLow,
                    ConflictChance = PersonalityEffectLevels.PositiveLow
                },
                PersonalityTraits.Apathetic => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeHigh,
                    Creativity = PersonalityEffectLevels.NegativeMedium,
                    Performance = PersonalityEffectLevels.NegativeMedium,
                    Morale = PersonalityEffectLevels.NegativeMax,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Overcritical => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMedium,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Withdrawn => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMax,
                    Creativity = PersonalityEffectLevels.NegativeLow,
                    Performance = PersonalityEffectLevels.NegativeMedium,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMedium
                },
                PersonalityTraits.Neglectful => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeHigh,
                    Creativity = PersonalityEffectLevels.NegativeMedium,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Overambitious => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.None,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeHigh,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },
                PersonalityTraits.Hotheaded => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMedium,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.None,
                    Morale = PersonalityEffectLevels.NegativeHigh,
                    ConflictChance = PersonalityEffectLevels.PositiveMax
                },
                PersonalityTraits.Manipulative => new PersonalityEffect
                {
                    Loyalty = PersonalityEffectLevels.NegativeMax,
                    Creativity = PersonalityEffectLevels.None,
                    Performance = PersonalityEffectLevels.NegativeLow,
                    Morale = PersonalityEffectLevels.NegativeMedium,
                    ConflictChance = PersonalityEffectLevels.PositiveHigh
                },

                _ => new PersonalityEffect()
            };

            return new PersonalityDeltas
            {
                PerformanceDelta = PersonalityDeltaCalculator.ToPerformanceDelta(effect.Performance),
                CreativityDelta = PersonalityDeltaCalculator.ToCreativityDelta(effect.Creativity),
                MoraleDelta = PersonalityDeltaCalculator.ToMoraleDelta(effect.Morale),
                ConflictChanceDelta = PersonalityDeltaCalculator.ToConflictDelta(effect.ConflictChance),
                LoyaltyDelta = PersonalityDeltaCalculator.ToLoyaltyDelta(effect.Loyalty)
            };
        }
    }
}