using System.Collections.Generic;

namespace Encore.Model.Band
{
    public record Band(
        string Name,
        List<BandMember.BandMember> Members,
        BandGenre Genre,
        BandPopularityLevels PopularityLevel,
        BandStatus Status,
        List<Band> Rivals,
        List<Band> Influences,
        List<Band> Supporting,
        List<Band> SupportedBy
    )
    {
        public string Name { get; set; }
        public List<BandMember.BandMember> Members { get; set; }
        public BandGenre Genre { get; set; }
        public BandPopularityLevels PopularityLevel { get; set; }
        public BandStatus Status { get; set; }
        public List<Band> Rivals { get; set; }
        public List<Band> Influences { get; set; }
        public List<Band> Supporting { get; set; }
        public List<Band> SupportedBy { get; set; }
    }
}