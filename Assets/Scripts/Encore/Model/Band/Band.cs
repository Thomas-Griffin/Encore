using System.Collections.Generic;

namespace Encore.Model.Band
{
    public class Band
    {
        public string Name;
        public List<BandMember.BandMember> Members;
        public BandGenre Genre;
        public BandPopularityLevels popularityLevel;
        public BandStatus status;
        public List<Band> Rivals;
        public List<Band> Influences;
        public List<Band> Supporting;
        public List<Band> SupportedBy;
    }
}