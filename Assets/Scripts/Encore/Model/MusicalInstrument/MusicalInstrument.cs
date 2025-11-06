namespace Encore.Model.MusicalInstrument
{
    public class MusicalInstrument
    {
        public MusicalInstrumentNames Name { get; set; }
        public MusicalInstrumentType Type { get; set; }
        public MusicalInstrumentQuality Quality { get; set; }
        public MusicalInstrumentCondition Condition { get; set; }
    }
}