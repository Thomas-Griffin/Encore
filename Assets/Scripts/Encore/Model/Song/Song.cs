namespace Encore.Model.Song
{
    public class Song
    {
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public string Genre { get; set; }
        public SongPopularityLevels Popularity { get; set; }
        public SongComplexityLevels Complexity { get; set; }
        public int EnergyRequired { get; set; }
        public int RevenuePotential { get; set; }
        public int PracticeTimeInHours { get; set; }
    }
}