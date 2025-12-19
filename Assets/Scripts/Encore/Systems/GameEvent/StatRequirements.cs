using Encore.Abstractions.Interfaces;
using Encore.Systems.Core;

namespace Encore.Systems.GameEvent
{
    public sealed class StatRequirements
    {
        public int MinEnergy { get; set; } = 0;
        public int MinSkill { get; set; } = 0;
        public int MinPopularity { get; set; } = 0;
        public int MinFame { get; set; } = 0;

        public bool AreMetBy(GameSession session,
            IStatService statService)
        {
            if (statService == null) return false;

            return statService.Energy.CurrentValue >= MinEnergy &&
                   statService.Skill.CurrentValue >= MinSkill &&
                   statService.Popularity.CurrentValue >= MinPopularity &&
                   statService.Fame.CurrentValue >= MinFame;
        }
    }
}