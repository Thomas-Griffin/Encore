namespace Encore.Model.Song
{
    public record RecordedSong(
        string Title,
        int DurationInSeconds,
        string Genre,
        SongPopularityLevels Popularity,
        SongComplexityLevels Complexity,
        int EnergyRequired,
        int RevenuePotential,
        int PracticeTimeInHours,
        int RecordingCost,
        int RoyaltyRate,
        bool IsSingle,
        bool HasMusicVideo,
        int MarketingBudget)
        : Song(
            Title,
            DurationInSeconds,
            Genre,
            Popularity,
            Complexity,
            EnergyRequired,
            RevenuePotential,
            PracticeTimeInHours
        )
    {
        public int RecordingCost { get; set; } = RecordingCost;
        public int RoyaltyRate { get; set; } = RoyaltyRate;
        public bool IsSingle { get; set; } = IsSingle;
        public bool HasMusicVideo { get; set; } = HasMusicVideo;
        public int MarketingBudget { get; set; } = MarketingBudget;
    }
}