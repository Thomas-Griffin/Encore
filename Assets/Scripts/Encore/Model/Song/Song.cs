namespace Encore.Model.Song
{
    public record Song(
        string Title,
        int DurationInSeconds,
        string Genre,
        SongPopularityLevels Popularity,
        SongComplexityLevels Complexity,
        int EnergyRequired,
        int RevenuePotential,
        int PracticeTimeInHours)
    {
        public string Title { get; set; } = Title;
        public int DurationInSeconds { get; set; } = DurationInSeconds;
        public string Genre { get; set; } = Genre;
        public SongPopularityLevels Popularity { get; set; } = Popularity;
        public SongComplexityLevels Complexity { get; set; } = Complexity;
        public int EnergyRequired { get; set; } = EnergyRequired;
        public int RevenuePotential { get; set; } = RevenuePotential;
        public int PracticeTimeInHours { get; set; } = PracticeTimeInHours;
    }
}