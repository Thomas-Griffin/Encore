namespace Encore.Model.MusicalInstrument
{
    public record MusicalInstrument(
        MusicalInstrumentNames Name,
        MusicalInstrumentType Type,
        MusicalInstrumentQuality Quality,
        MusicalInstrumentCondition Condition,
        int Value)
    {
        public MusicalInstrumentNames Name { get; set; } = Name;
        public MusicalInstrumentType Type { get; set; } = Type;
        public MusicalInstrumentQuality Quality { get; set; } = Quality;
        public MusicalInstrumentCondition Condition { get; set; } = Condition;
        public int Value { get; set; } = Value;
    }
}