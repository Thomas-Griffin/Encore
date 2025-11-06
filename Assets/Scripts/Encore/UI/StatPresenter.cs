using System.Collections.Generic;
using System.Linq;
using Encore.Model.Stats;
using Encore.Systems;
using UnityEngine;

namespace Encore.UI
{
    public class StatPresenter
    {
        private readonly StatManager _statManager;
        private readonly Dictionary<string, float> _displayedValues = new();

        // units per second
        private const float AnimationSpeed = 30f;

        public StatPresenter(StatManager statManager)
        {
            _statManager = statManager;
            if (_statManager) InitializeDisplayedValues();
        }

        private void InitializeDisplayedValues()
        {
            GameStat[] stats = _statManager?.GetStats();
            if (stats == null) return;

            foreach (GameStat gameStat in stats)
            {
                if (gameStat == null) continue; // defensive: skip null entries

                _displayedValues[gameStat.Stat.ToString()] = gameStat.CurrentValue;
            }
        }

        public void Update(float deltaTime)
        {
            if (!_statManager) return;

            // cache stats and guard against null returns
            GameStat[] stats = _statManager?.GetStats();
            if (stats != null)
            {
                foreach (GameStat gameStat in stats)
                {
                    if (gameStat == null) continue; // defensive

                    string key = gameStat.Stat.ToString();
                    float target = gameStat.CurrentValue;

                    if (!_displayedValues.TryGetValue(key, out float displayed))
                    {
                        _displayedValues[key] = target;
                        continue;
                    }

                    if (Mathf.Approximately(displayed, target)) continue;

                    _displayedValues[key] = Mathf.MoveTowards(displayed, target, AnimationSpeed * deltaTime);
                }
            }

            // cleanup removed stats
            HashSet<string> existingKeys = new();
            GameStat[] existingStats = _statManager?.GetStats();
            if (existingStats != null)
            {
                foreach (GameStat gameStat in existingStats)
                {
                    if (gameStat == null) continue;
                    existingKeys.Add(gameStat.Stat.ToString());
                }
            }

            List<string> toRemove = _displayedValues.Keys.Where(key => !existingKeys.Contains(key)).ToList();

            foreach (string key in toRemove) _displayedValues.Remove(key);
        }

        public float GetDisplayedValue(GameStat stat)
        {
            if (stat == null) return 0f;
            if (_displayedValues.TryGetValue(stat.Stat.ToString(), out float v)) return v;
            return stat.CurrentValue;
        }
    }
}