using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.GameEvent;

namespace Encore.Abstractions.Interfaces
{
    public interface IStatService
    {
        GameStat Energy { get; }
        GameStat Skill { get; }
        GameStat Popularity { get; }
        GameStat Fame { get; }
        
        GameStat[] GetStats();

        void InitialiseStats(Difficulty difficulty);
        void ApplyDeltas(StatDeltas deltas);

        bool WinConditionMet();
        bool LoseConditionMet();
    }
}