namespace Encore.Systems.Personality
{
    public struct PersonalityEffect
    {
        public PersonalityEffectLevels Performance;
        public PersonalityEffectLevels Creativity;
        public PersonalityEffectLevels Morale;
        public PersonalityEffectLevels ConflictChance;
        public PersonalityEffectLevels Loyalty;

        public static PersonalityEffect Create(
            PersonalityEffectLevels performance = PersonalityEffectLevels.None,
            PersonalityEffectLevels creativity = PersonalityEffectLevels.None,
            PersonalityEffectLevels morale = PersonalityEffectLevels.None,
            PersonalityEffectLevels conflictChance = PersonalityEffectLevels.None,
            PersonalityEffectLevels loyalty = PersonalityEffectLevels.None)
        {
            return new PersonalityEffect
            {
                Performance = performance,
                Creativity = creativity,
                Morale = morale,
                ConflictChance = conflictChance,
                Loyalty = loyalty
            };
        }

        public PersonalityDeltas ToNumeric()
        {
            return new PersonalityDeltas
            {
                PerformanceDelta = PersonalityDeltaCalculator.ToPerformanceDelta(Performance),
                CreativityDelta = PersonalityDeltaCalculator.ToCreativityDelta(Creativity),
                MoraleDelta = PersonalityDeltaCalculator.ToMoraleDelta(Morale),
                ConflictChanceDelta = PersonalityDeltaCalculator.ToConflictDelta(ConflictChance),
                LoyaltyDelta = PersonalityDeltaCalculator.ToLoyaltyDelta(Loyalty)
            };
        }
    }
}
