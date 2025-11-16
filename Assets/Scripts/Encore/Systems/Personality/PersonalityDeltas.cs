namespace Encore.Systems.Personality
{
    public struct PersonalityDeltas
    {
        public float PerformanceDelta; // affects show quality
        public float CreativityDelta; // affects creativity-related tasks
        public float MoraleDelta; // affects morale bonuses
        public float ConflictChanceDelta; // affects probability of conflict
        public float LoyaltyDelta; // affects chance to stay/leave

        public static PersonalityDeltas operator +(PersonalityDeltas a, PersonalityDeltas b)
        {
            return new PersonalityDeltas
            {
                PerformanceDelta = a.PerformanceDelta + b.PerformanceDelta,
                CreativityDelta = a.CreativityDelta + b.CreativityDelta,
                MoraleDelta = a.MoraleDelta + b.MoraleDelta,
                ConflictChanceDelta = a.ConflictChanceDelta + b.ConflictChanceDelta,
                LoyaltyDelta = a.LoyaltyDelta + b.LoyaltyDelta
            };
        }
    }
}