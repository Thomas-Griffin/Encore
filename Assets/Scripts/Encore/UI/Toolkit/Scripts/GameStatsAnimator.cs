using Encore.Abstractions.Interfaces;
using Encore.Model.Stats;
using UnityEngine;

namespace Encore.UI.Toolkit.Scripts
{
    public class GameStatsAnimator
    {
        private readonly IStatService _stats;
        private const float AnimationDurationSeconds = 2f;
        private const float FinishEpsilonPercent = 0.01f;

        public GameStatsAnimator(IStatService stats)
        {
            _stats = stats;
        }

        public void Update(float deltaTime)
        {
            GameStat[] stats = _stats?.GetStats();
            if (stats == null) return;
            foreach (GameStat gameStat in stats)
            {
                if (gameStat == null) continue;

                if (gameStat.MaxValue <= 0)
                {
                    gameStat.DisplayValue = gameStat.CurrentValue;
                    gameStat.LastValue = gameStat.CurrentValue;
                    continue;
                }

                float speed = gameStat.MaxValue / AnimationDurationSeconds;

                float epsilon = Mathf.Max(FinishEpsilonPercent * gameStat.MaxValue, 0.001f);

                if (Mathf.Abs(gameStat.DisplayValue - gameStat.CurrentValue) <= epsilon)
                {
                    gameStat.DisplayValue = gameStat.CurrentValue;
                    gameStat.LastValue = gameStat.CurrentValue;
                    continue;
                }

                gameStat.DisplayValue = Mathf.MoveTowards(gameStat.DisplayValue, gameStat.CurrentValue,
                    speed * deltaTime);
                gameStat.LastValue = Mathf.RoundToInt(gameStat.DisplayValue);
            }
        }
    }
}