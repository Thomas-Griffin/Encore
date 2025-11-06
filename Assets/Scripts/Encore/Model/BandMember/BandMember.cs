using System.Collections.Generic;

namespace Encore.Model.BandMember
{
    public record BandMember(
        string Name,
        string Surname,
        string Nickname,
        BandMemberRoles MemberRole,
        BandMemberMoraleLevels Morale,
        BandMemberLoyaltyLevels Loyalty,
        List<BandMemberMusicalInstrument> OwnedInstruments,
        List<PersonalityTraits> PersonalityTraits)
    {
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string Nickname { get; set; } = Nickname;
        public List<BandMemberMusicalInstrument> OwnedInstruments { get; set; } = OwnedInstruments;
        public BandMemberMusicalInstrument CurrentInstrument { get; set; }
        public BandMemberRoles MemberRole { get; set; } = MemberRole;
        public BandMemberMoraleLevels Morale { get; set; } = Morale;
        public BandMemberLoyaltyLevels Loyalty { get; set; } = Loyalty;
        public List<PersonalityTraits> PersonalityTraits { get; set; } = PersonalityTraits;
    }
}