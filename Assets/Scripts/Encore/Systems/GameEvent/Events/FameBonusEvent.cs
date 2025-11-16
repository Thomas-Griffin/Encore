using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    public class FameBonusEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.StatChanged;

        public override string EventName => "FameBonus";

        public override string Description => "You received a boost in fame!";

        public FameBonusEvent(DifficultyLevel difficulty = DifficultyLevel.Easy)
        {
            Requirements = new StatRequirements
            {
                MinEnergy = 0,
                MinSkill = difficulty switch
                {
                    DifficultyLevel.Easy => 5,
                    DifficultyLevel.Medium => 6,
                    DifficultyLevel.Hard => 7,
                    _ => 5
                },
                MinPopularity = 1,
                MinFame = 0
            };
        }

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = 0;
            Deltas.skillDelta = 0;
            Deltas.popularityDelta = 0;
            Deltas.fameDelta = CalculateFameDelta(state);
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }

        private int CalculateFameDelta(GameInstance state)
        {
            return state.Stats.Skill.CurrentValue * state.Stats.Popularity.CurrentValue * ConsecutiveEventRepetitions / state.Stats.Fame.MaxValue;
        }
    }
}