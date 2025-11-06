using System.Collections.Generic;

namespace Encore.Model.Tour
{
    public class Gig
    {
        public Venue Venue { get; set; }
        public AudienceSizes ExpectedAudienceSize { get; set; }
        public AudienceSizes ActualAudienceSize { get; set; }
        public int DurationInMinutes { get; set; }
        public int MinimumSetlistSize { get; set; }
        public List<Song.Song> Setlist { get; set; }
        public int TicketPrice { get; set; }
        public Dictionary<ExpenseTypes, List<int>> Expenses { get; set; }
        public Dictionary<EarningTypes, List<int>> Earnings { get; set; }
    }
}