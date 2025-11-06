namespace Encore.Model.Tour
{
    public class Venue
    {
        public string Name { get; set; }
        public VenueTypes Type { get; set; }
        public int Capacity { get; set; }
        public int BookingFee { get; set; }
    }
}