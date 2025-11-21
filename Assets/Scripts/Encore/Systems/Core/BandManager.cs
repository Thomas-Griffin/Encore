using System.Collections.Generic;
using Encore.Model.Band;
using Encore.Model.BandMember;
using Encore.Model.MusicalInstrument;
using Encore.Systems.Personality;

namespace Encore.Systems.Core
{
    public class BandManager
    {
        public BandManager(List<Band> playerBands)
        {
            PlayerBands = playerBands;
        }

        private List<Band> PlayerBands { get; set; }

        public void AddNewBand(
            string name
        )
        {
            Band band = new(
                name: name,
                members: new List<BandMember>(),
                genre: BandGenre.Rock,
                fameLevel: BandFameLevels.Unknown,
                status: BandStatus.Inactive,
                rivals: new List<Band>(),
                influences: new List<Band>(),
                supporting: new List<Band>(),
                supportedBy: new List<Band>()
            );
            PlayerBands.Add(band);
        }

        public void AddMemberToBand(
            Band band,
            string memberName,
            string memberSurname,
            string memberNickname,
            BandMemberRoles memberRole,
            BandMemberMoraleLevels morale,
            BandMemberLoyaltyLevels loyalty,
            Dictionary<BandMemberSkillLevel, List<MusicalInstrument>> ownedInstruments,
            List<PersonalityTraits> personalityTraits
        )
        {
            List<PersonalityTraits> normalizedTraits =
                PersonalityAggregator.NormaliseTraits(personalityTraits ?? new List<PersonalityTraits>());

            BandMember newMember = new(
                Name: memberName,
                Surname: memberSurname,
                Nickname: memberNickname,
                MemberRole: memberRole,
                Morale: morale,
                Loyalty: loyalty,
                OwnedInstruments: ownedInstruments,
                PersonalityTraits: normalizedTraits
            );
            band.Members.Add(newMember);
        }

        public void RemoveMemberFromBand(
            Band band,
            BandMember member
        )
        {
            if (band.Members.Contains(member))
            {
                band.Members.Remove(member);
            }
        }

        public void ChangeBandGenre(
            Band band,
            BandGenre newGenre
        )
        {
            band.Genre = newGenre;
        }
    }
}