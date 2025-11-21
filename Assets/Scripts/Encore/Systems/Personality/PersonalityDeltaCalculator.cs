namespace Encore.Systems.Personality
{
    public static class PersonalityDeltaCalculator
    {
        private static float PerformanceGain { get; set; } = 0.03f;
        private static float CreativityGain { get; set; } = 0.05f;
        private static float MoraleGain { get; set; } = 0.04f;
        private static float ConflictChanceGain { get; set; } = 0.03f;
        private static float LoyaltyGain { get; set; } = 0.02f;

        public static float ToPerformanceDelta(PersonalityEffectLevels effectLevel) => (int)effectLevel * PerformanceGain;
        public static float ToCreativityDelta(PersonalityEffectLevels effectLevel) => (int)effectLevel * CreativityGain;
        public static float ToMoraleDelta(PersonalityEffectLevels effectLevel) => (int)effectLevel * MoraleGain;
        public static float ToConflictDelta(PersonalityEffectLevels effectLevel) => (int)effectLevel * ConflictChanceGain;
        public static float ToLoyaltyDelta(PersonalityEffectLevels effectLevel) => (int)effectLevel * LoyaltyGain;

        public static PersonalityDeltas ToNumericDelta(PersonalityEffect tiered)
        {
            return new PersonalityDeltas
            {
                PerformanceDelta = ToPerformanceDelta(tiered.Performance),
                CreativityDelta = ToCreativityDelta(tiered.Creativity),
                MoraleDelta = ToMoraleDelta(tiered.Morale),
                ConflictChanceDelta = ToConflictDelta(tiered.ConflictChance),
                LoyaltyDelta = ToLoyaltyDelta(tiered.Loyalty)
            };
        }
    }
}
