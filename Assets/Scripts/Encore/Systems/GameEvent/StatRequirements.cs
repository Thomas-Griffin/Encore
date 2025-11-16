using Encore.Systems.Core;

namespace Encore.Systems.GameEvent
{
    public class StatRequirements
    {
        public int MinEnergy { get; set; } = 0;
        public int MinSkill { get; set; } = 0;
        public int MinPopularity { get; set; } = 0;
        public int MinFame { get; set; } = 0;

        public bool AreMetBy(StatManager stats)
        {
            return stats.Energy.CurrentValue >= MinEnergy &&
                   stats.Skill.CurrentValue >= MinSkill &&
                   stats.Popularity.CurrentValue >= MinPopularity &&
                   stats.Fame.CurrentValue >= MinFame;
        }
    }
}