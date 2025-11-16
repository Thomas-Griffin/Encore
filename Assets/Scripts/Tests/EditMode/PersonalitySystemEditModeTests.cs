using System.Collections.Generic;
using Encore.Model.Band;
using Encore.Model.BandMember;
using Encore.Systems.Personality;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class PersonalitySystemTests
    {
        [Test]
        public void NormaliseTraits_Removes_Conflicts()
        {
            List<PersonalityTraits> traits = new()
            {
                PersonalityTraits.Lazy,
                PersonalityTraits.Diligent,
                PersonalityTraits.Lazy // Duplicate to test removal
            };

            List<PersonalityTraits> normalized = PersonalityAggregator.NormaliseTraits(traits);

            Assert.Contains(PersonalityTraits.Diligent, normalized);
            Assert.IsFalse(normalized.Contains(PersonalityTraits.Lazy));
            Assert.AreEqual(1, normalized.Count);
        }

        [Test]
        public void AggregateForMember_SumsModifiersCorrectly()
        {
            List<PersonalityTraits> traits = new()
            {
                PersonalityTraits.Creative,
                PersonalityTraits.Passionate
            };

            BandMember member = new(
                Name: "Test",
                Surname: "Tested",
                Nickname: "T",
                MemberRole: BandMemberRoles.LeadGuitarist,
                Morale: BandMemberMoraleLevels.Medium,
                Loyalty: BandMemberLoyaltyLevels.Medium,
                OwnedInstruments: new List<BandMemberMusicalInstrument>(),
                PersonalityTraits: traits
            );

            PersonalityDeltas mods = PersonalityAggregator.AggregateForBandMember(member);

            Assert.AreEqual(0.40f, mods.CreativityDelta, 0.0001f);
            Assert.AreEqual(0.18, mods.PerformanceDelta, 0.0001f);
        }

        [Test]
        public void ModifierScale_Converts_Tiers_To_Numeric()
        {
            Assert.AreEqual(0.06f, PersonalityDeltaCalculator.ToPerformanceDelta(PersonalityEffectLevels.PositiveMedium), 0.0001f);
            Assert.AreEqual(-0.03f, PersonalityDeltaCalculator.ToPerformanceDelta(PersonalityEffectLevels.NegativeLow), 0.0001f);
            Assert.AreEqual(0.15f, PersonalityDeltaCalculator.ToCreativityDelta(PersonalityEffectLevels.PositiveHigh), 0.0001f);
        }

        [Test]
        public void Tiered_ToNumeric_Matches_ModifierScale()
        {
            PersonalityEffect personalityEffect = PersonalityEffect.Create(
                performance: PersonalityEffectLevels.PositiveMedium,
                creativity: PersonalityEffectLevels.PositiveHigh,
                morale: PersonalityEffectLevels.PositiveLow,
                conflictChance: PersonalityEffectLevels.NegativeLow,
                loyalty: PersonalityEffectLevels.PositiveMedium
            );

            PersonalityDeltas numeric = personalityEffect.ToNumeric();

            Assert.AreEqual(PersonalityDeltaCalculator.ToPerformanceDelta(PersonalityEffectLevels.PositiveMedium), numeric.PerformanceDelta, 0.0001f);
            Assert.AreEqual(PersonalityDeltaCalculator.ToCreativityDelta(PersonalityEffectLevels.PositiveHigh), numeric.CreativityDelta, 0.0001f);
            Assert.AreEqual(PersonalityDeltaCalculator.ToMoraleDelta(PersonalityEffectLevels.PositiveLow), numeric.MoraleDelta, 0.0001f);
            Assert.AreEqual(PersonalityDeltaCalculator.ToConflictDelta(PersonalityEffectLevels.NegativeLow), numeric.ConflictChanceDelta, 0.0001f);
            Assert.AreEqual(PersonalityDeltaCalculator.ToLoyaltyDelta(PersonalityEffectLevels.PositiveMedium), numeric.LoyaltyDelta, 0.0001f);
        }

        [Test]
        public void PersonalityTraitMap_Creative_Mapping_Returns_Expected_Numeric()
        {
            PersonalityDeltas personalityDeltas = PersonalityTraitBonusMap.For(PersonalityTraits.Creative);
            Assert.AreEqual(0.2, personalityDeltas.CreativityDelta, 0.0001f,
                "Creative should give creativity bonus");
            Assert.AreEqual(0.09f, personalityDeltas.PerformanceDelta, 0.0001f,
                "Creative should give small performance bonus");
        }

        [Test]
        public void PersonalityNormaliser_Removes_Conflicts_And_Preserves_Order()
        {
            List<PersonalityTraits> personalityTraitsList = new()
            {
                PersonalityTraits.Optimistic,
                PersonalityTraits.Pessimistic,
                PersonalityTraits.Loyal,
                PersonalityTraits.Manipulative
            };

            List<PersonalityTraits> normalised = PersonalityTraitNormaliser.Normalise(personalityTraitsList);

            Assert.IsFalse(normalised.Contains(PersonalityTraits.Pessimistic));
            Assert.IsFalse(normalised.Contains(PersonalityTraits.Manipulative));

            Assert.AreEqual(PersonalityTraits.Optimistic, normalised[0]);
            Assert.AreEqual(PersonalityTraits.Loyal, normalised[1]);
        }

        [Test]
        public void EvaluateShow_NoTraits_Returns_Base_Quality()
        {
            BandMember bandMember = new(
                Name: "A",
                Surname: "One",
                Nickname: "A",
                MemberRole: BandMemberRoles.Drummer,
                Morale: BandMemberMoraleLevels.Medium,
                Loyalty: BandMemberLoyaltyLevels.Medium,
                OwnedInstruments: new List<BandMemberMusicalInstrument>(),
                PersonalityTraits: new List<PersonalityTraits>()
            );
            BandMember secondBandMember = new(
                Name: "B",
                Surname: "Two",
                Nickname: "B",
                MemberRole: BandMemberRoles.LeadGuitarist,
                Morale: BandMemberMoraleLevels.Medium,
                Loyalty: BandMemberLoyaltyLevels.Medium,
                OwnedInstruments: new List<BandMemberMusicalInstrument>(),
                PersonalityTraits: new List<PersonalityTraits>()
            );

            Band band = new(
                name: "TestBand",
                members: new List<BandMember> { bandMember, secondBandMember },
                genre: BandGenre.Rock,
                popularityLevel: BandPopularityLevels.Unknown,
                status: BandStatus.OnTour,
                rivals: new List<Band>(),
                influences: new List<Band>(),
                supporting: new List<Band>(),
                supportedBy: new List<Band>()
            );

            float quality = PersonalitySimulator.EvaluateGigUsingPersonalityTraits(band);
            Assert.AreEqual(0f, quality, 0.01f);
        }

        [Test]
        public void ApplyPostGigMorale_With_Positive_Trait()
        {
            BandMember bandMember = new(
                Name: "Res",
                Surname: "lient",
                Nickname: "I",
                MemberRole: BandMemberRoles.Bassist,
                Morale: BandMemberMoraleLevels.Medium,
                Loyalty: BandMemberLoyaltyLevels.Medium,
                OwnedInstruments: new List<BandMemberMusicalInstrument>(),
                PersonalityTraits: new List<PersonalityTraits> { PersonalityTraits.Resilient }
            );

            Band band = new(
                name: "ResilientBand",
                members: new List<BandMember> { bandMember },
                genre: BandGenre.Rock,
                popularityLevel: BandPopularityLevels.Unknown,
                status: BandStatus.OnTour,
                rivals: new List<Band>(),
                influences: new List<Band>(),
                supporting: new List<Band>(),
                supportedBy: new List<Band>()
            );

            Assert.NotNull(band, "band should not be null after construction");
            Assert.NotNull(band.Members, "band.Members should not be null after construction");
            Assert.IsNotEmpty(band.Members, "band.Members should not be empty after construction");
            Assert.NotNull(band.Members[0], "band.Members[0] should not be null after construction");

            BandMember memberFromBand = band.Members[0];

            PersonalityDeltas deltas = PersonalityAggregator.AggregateForBandMember(memberFromBand);
            TestContext.WriteLine($"Computed morale delta = {deltas.MoraleDelta}");
            float expectedMoraleDelta = PersonalityDeltaCalculator.ToMoraleDelta(PersonalityEffectLevels.PositiveMax);
            Assert.AreEqual(expectedMoraleDelta, deltas.MoraleDelta, 0.0001f);

            const float tierSize = 0.25f;
            float adjustedModifier = 1f - Mathf.Clamp01(deltas.MoraleDelta);
            float finalChange = 1f * tierSize * adjustedModifier;
            float currentMoraleValue = 0.5f;
            float updatedNormalized = Mathf.Clamp01(currentMoraleValue + finalChange);
            TestContext.WriteLine($"adjustedModifier={adjustedModifier} finalChange={finalChange} updatedNormalized={updatedNormalized}");
            Assert.Greater(finalChange, 0.2f);
            Assert.Greater(updatedNormalized, 0.5f);

            string beforeMoraleStr = memberFromBand.Morale.ToString();
            TestContext.WriteLine($"morale before apply = {beforeMoraleStr}");

            PersonalitySimulator.ApplyPostGigMoraleToBandMembers(band, 1f);

            string afterMoraleStr = band.Members[0].Morale.ToString();
            TestContext.WriteLine($"morale after apply = {afterMoraleStr}");

            // Map actual assigned morale back to normalized value for diagnostics
            float ActualMoraleToFloat(BandMemberMoraleLevels morale)
            {
                return morale switch
                {
                    BandMemberMoraleLevels.VeryLow => 0.0f,
                    BandMemberMoraleLevels.Low => 0.25f,
                    BandMemberMoraleLevels.Medium => 0.5f,
                    BandMemberMoraleLevels.High => 0.75f,
                    BandMemberMoraleLevels.VeryHigh => 1.0f,
                    _ => 0.5f
                };
            }

            float actualNormalized = ActualMoraleToFloat(band.Members[0].Morale);
            TestContext.WriteLine($"expectedNormalized={updatedNormalized} actualNormalized(from assigned tier)={actualNormalized}");

            BandMemberMoraleLevels expectedMorale;
            if (updatedNormalized <= 0f) expectedMorale = BandMemberMoraleLevels.VeryLow;
            else if (updatedNormalized <= 0.25f) expectedMorale = BandMemberMoraleLevels.Low;
            else if (updatedNormalized <= 0.5f) expectedMorale = BandMemberMoraleLevels.Medium;
            else if (updatedNormalized <= 0.75f) expectedMorale = BandMemberMoraleLevels.High;
            else expectedMorale = BandMemberMoraleLevels.VeryHigh;

            Assert.AreEqual(expectedMorale, band.Members[0].Morale,
                $"Expected band.Members[0].Morale == {expectedMorale} based on updatedNormalized={updatedNormalized} but was {band.Members[0].Morale} (actualNormalized={actualNormalized})");
        }
    }
}