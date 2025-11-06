using System.Collections.Generic;

namespace Encore.Model.BandMember
{
    public class BandMember
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public List<BandMemberMusicalInstrument> OwnedInstruments { get; set; }
        public BandMemberMusicalInstrument CurrentInstrument { get; set; }
        public BandMemberRoles MemberRole { get; set; }
        public BandMemberMoraleLevels Morale { get; set; }
        public BandMemberLoyaltyLevels Loyalty { get; set; }
        public List<PersonalityTraits> PersonalityTraits { get; set; }
    }
}