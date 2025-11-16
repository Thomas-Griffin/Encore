using System.Collections.Generic;
using Encore.Model.Band;
using Encore.Model.BandMember;
using Encore.Systems.Personality;
using UnityEngine;

namespace Encore.Systems.Core
{
    public class BandManager
    {
        public BandManager(List<Band> playerBands)
        {
            PlayerBands = playerBands;
        }

        public List<Band> PlayerBands { get; set; }

        public void AddNewBand(
            string name
        )
        {
            Band band = new(
                name: name,
                members: new List<BandMember>(),
                genre: BandGenre.Rock,
                popularityLevel: BandPopularityLevels.Unknown,
                status: BandStatus.Inactive,
                rivals: new List<Band>(),
                influences: new List<Band>(),
                supporting: new List<Band>(),
                supportedBy: new List<Band>()
            );
            PlayerBands.Add(band);
            Debug.Log("BandManager: Added new band " + name);
        }

        public void AddMemberToBand(
            Band band,
            string memberName,
            string memberSurname,
            string memberNickname,
            BandMemberRoles memberRole,
            BandMemberMoraleLevels morale,
            BandMemberLoyaltyLevels loyalty,
            List<BandMemberMusicalInstrument> ownedInstruments,
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
            Debug.Log("BandManager: Added new member " + memberName + " to band " + band.Name);
        }

        public void RemoveMemberFromBand(
            Band band,
            BandMember member
        )
        {
            if (band.Members.Contains(member))
            {
                band.Members.Remove(member);
                Debug.Log("BandManager: Removed member " + member.Name + " from band " + band.Name);
            }
            else
            {
                Debug.LogWarning("BandManager: Member " + member.Name + " not found in band " + band.Name);
            }
        }

        public void ChangeBandGenre(
            Band band,
            BandGenre newGenre
        )
        {
            band.Genre = newGenre;
            Debug.Log("BandManager: Changed genre of band " + band.Name + " to " + newGenre);
        }
    }
}