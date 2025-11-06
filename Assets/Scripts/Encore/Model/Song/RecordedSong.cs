namespace Encore.Model.Song
{
    public class RecordedSong : Song
    {
        public int RecordingCost { get; set; }
        public int RoyaltyRate { get; set; }
        public bool IsSingle { get; set; }
        public bool HasMusicVideo { get; set; }
        public int MarketingBudget { get; set; }
    }
}