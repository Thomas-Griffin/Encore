using System.Collections.Generic;

namespace Encore.Model.Band
{
    public class Band
    {
        public string Name { get; set; }
        public List<BandMember.BandMember> Members { get; set; }
        public BandGenre Genre { get; set; }
        public BandFameLevels FameLevel { get; set; }
        public BandStatus Status { get; set; }
        public List<Band> Rivals { get; set; }
        public List<Band> Influences { get; set; }
        public List<Band> Supporting { get; set; }
        public List<Band> SupportedBy { get; set; }

        public Band(string name,
            List<BandMember.BandMember> members,
            BandGenre genre,
            BandFameLevels fameLevel,
            BandStatus status,
            List<Band> rivals,
            List<Band> influences,
            List<Band> supporting,
            List<Band> supportedBy)
        {
            Name = name;
            Members = members ?? new List<BandMember.BandMember>();
            Genre = genre;
            FameLevel = fameLevel;
            Status = status;
            Rivals = rivals ?? new List<Band>();
            Influences = influences ?? new List<Band>();
            Supporting = supporting ?? new List<Band>();
            SupportedBy = supportedBy ?? new List<Band>();
        }
    }
}