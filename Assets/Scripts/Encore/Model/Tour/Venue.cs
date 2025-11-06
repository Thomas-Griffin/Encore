namespace Encore.Model.Tour
{
    public record Venue(
        string Name,
        VenueTypes Type,
        int Capacity,
        int BookingFee
    )
    {
        public string Name { get; set; } = Name;
        public VenueTypes Type { get; set; } = Type;
        public int Capacity { get; set; } = Capacity;
        public int BookingFee { get; set; } = BookingFee;
    }
}