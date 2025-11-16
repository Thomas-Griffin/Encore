using Encore.Model.Stats;
using Encore.Systems.Core;
using UnityEngine;

namespace Encore.UI.Helpers
{
    public class StatPresenter
    {
        private readonly StatManager _statManager;

        private const float AnimationDurationSeconds = 0.01f;

        private const float FinishEpsilonPercent = 0.001f;

        public StatPresenter(StatManager statManager)
        {
            _statManager = statManager;
        }

        public void Update(float deltaTime)
        {
            if (!_statManager) return;
            GameStat[] stats = _statManager.GetStats();
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