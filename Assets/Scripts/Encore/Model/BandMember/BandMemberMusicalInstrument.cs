namespace Encore.Model.BandMember
{
    public record BandMemberMusicalInstrument(
        MusicalInstrument.MusicalInstrument Instrument,
        BandMemberSkillLevel SkillLevel)
    {
        private MusicalInstrument.MusicalInstrument Instrument { get; set; } = Instrument;
        private BandMemberSkillLevel SkillLevel { get; set; } = SkillLevel;
    }
}