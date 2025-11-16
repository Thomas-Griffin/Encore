using System;
using Encore.Model.Game;

namespace Encore.Systems.GameEvent.Events
{
    public class PractiseEvent : GameEventBase
    {
        public override EventTypes Type { get; set; } = EventTypes.ActionTaken;

        public override string EventName => "Practise";

        public override string Description => "You spent time practising your skills.";

        public PractiseEvent()
        {
            Requirements = new StatRequirements
            {
                MinEnergy = 1 + Math.Abs(CalculateEnergyDelta()),
                MinSkill = 0,
                MinPopularity = 0,
                MinFame = 0
            };
        }

        public override GameEventBase Apply(GameInstance state)
        {
            Deltas.energyDelta = CalculateEnergyDelta();
            Deltas.skillDelta = CalculateSkillDelta();
            Deltas.popularityDelta = 0;
            Deltas.fameDelta = 0;
            if (!RequirementsAreMet(state)) return this;
            state.Stats.ApplyDeltas(Deltas);
            return this;
        }

        private int CalculateSkillDelta()
        {
            return 1 * ConsecutiveEventRepetitions;
        }

        private int CalculateEnergyDelta()
        {
            if (ConsecutiveEventRepetitions <= 5)
            {
                return -10 + (ConsecutiveEventRepetitions - 1) * 2;
            }

            return -5;
        }
    }
}