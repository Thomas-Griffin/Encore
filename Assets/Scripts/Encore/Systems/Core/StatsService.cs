using System.Collections.Generic;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Configurations;
using Encore.Systems.GameEvent;

namespace Encore.Systems.Core
{
    public sealed class StatsService : IStatService
    {
        private readonly StatsConfig _config;

        private int _fameTarget;

        public GameStat Energy { get; private set; }
        public GameStat Skill { get; private set; }
        public GameStat Popularity { get; private set; }
        public GameStat Fame { get; private set; }

        public StatsService(StatsConfig config)
        {
            _config = config;
        }

        public void InitialiseStats(Difficulty difficulty)
        {
            _fameTarget = _config.GetFameTarget(difficulty);

            Energy = new GameStat(GameStats.Energy, _config.energyColor, 100, 0, 100, 60, 60, 1, 1);
            Skill = new GameStat(GameStats.Skill, _config.skillColor, 0, 0, 10, 10, 10, 1, 1);
            Popularity = new GameStat(GameStats.Popularity, _config.popularityColor, 0, 0, 50, 5, 10, 1, 1);
            Fame = new GameStat(GameStats.Fame, _config.fameColor, 0, 0, _fameTarget, 40, 40, 1, 1);
        }

        public GameStat[] GetStats()
        {
            List<GameStat> list = new(4);
            if (Energy != null) list.Add(Energy);
            if (Skill != null) list.Add(Skill);
            if (Popularity != null) list.Add(Popularity);
            if (Fame != null) list.Add(Fame);
            return list.ToArray();
        }

        public bool LoseConditionMet() => Energy != null && Energy.IsAtMinimum();
        public bool WinConditionMet() => Fame != null && Fame.IsAtMaximum();

        public void ApplyDeltas(StatDeltas deltas)
        {
            if (deltas == null) return;
            ApplyDelta(GameStats.Energy, deltas.energyDelta);
            ApplyDelta(GameStats.Skill, deltas.skillDelta);
            ApplyDelta(GameStats.Popularity, deltas.popularityDelta);
            ApplyDelta(GameStats.Fame, deltas.fameDelta);
        }

        private void ApplyDelta(GameStats stat,
            int delta)
        {
            GameStat gameStat = GetStat(stat);
            if (gameStat == null) return;

            switch (delta)
            {
                case > 0:
                    gameStat.IncreaseBy(delta);
                    break;
                case < 0:
                    gameStat.DecreaseBy(-delta);
                    break;
            }
        }

        private GameStat GetStat(GameStats stat) => stat switch
        {
            GameStats.Energy => Energy,
            GameStats.Skill => Skill,
            GameStats.Popularity => Popularity,
            GameStats.Fame => Fame,
            _ => null
        };
    }
}