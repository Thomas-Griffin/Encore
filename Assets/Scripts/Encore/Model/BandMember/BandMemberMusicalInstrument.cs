namespace Encore.Model.BandMember
{
    public class BandMemberMusicalInstrument
    {
        public BandMemberMusicalInstrument(MusicalInstrument.MusicalInstrument instrument, BandMemberSkillLevel skillLevel)
        {
            Instrument = instrument;
            SkillLevel = skillLevel;
        }

        MusicalInstrument.MusicalInstrument Instrument { get; set; }
        BandMemberSkillLevel SkillLevel { get; set; }
    }
}