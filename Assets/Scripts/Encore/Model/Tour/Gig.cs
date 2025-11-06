using System.Collections.Generic;

namespace Encore.Model.Tour
{
    public record Gig(
        Venue Venue,
        AudienceSizes ExpectedAudienceSize,
        AudienceSizes ActualAudienceSize,
        int DurationInMinutes,
        int MinimumSetlistSize,
        List<Song.Song> Setlist,
        int TicketPrice,
        Dictionary<ExpenseTypes, List<int>> Expenses,
        Dictionary<EarningTypes, List<int>> Earnings
    )
    {
        public Venue Venue { get; set; } = Venue;
        public AudienceSizes ExpectedAudienceSize { get; set; } = ExpectedAudienceSize;
        public AudienceSizes ActualAudienceSize { get; set; } = ActualAudienceSize;
        public int DurationInMinutes { get; set; } = DurationInMinutes;
        public int MinimumSetlistSize { get; set; } = MinimumSetlistSize;
        public List<Song.Song> Setlist { get; set; } = Setlist;
        public int TicketPrice { get; set; } = TicketPrice;
        public Dictionary<ExpenseTypes, List<int>> Expenses { get; set; } = Expenses;
        public Dictionary<EarningTypes, List<int>> Earnings { get; set; } = Earnings;
    }
}