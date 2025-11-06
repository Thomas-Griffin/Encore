using System.Collections.Generic;
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
            var stats = _statManager?.GetStats();
            if (stats == null) return;

            foreach (var s in stats)
            {
                if (s == null) continue; // defensive: skip null entries

                // Stat is expected to be non-null/enum; use ToString() for the key
                _displayedValues[s.Stat.ToString()] = s.CurrentValue;
            }
        }

        public void Update(float deltaTime)
        {
            if (!_statManager) return;

            // cache stats and guard against null returns
            var stats = _statManager?.GetStats();
            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    if (stat == null) continue; // defensive

                    string key = stat.Stat.ToString();
                    float target = stat.CurrentValue;

                    if (!_displayedValues.TryGetValue(key, out var displayed))
                    {
                        _displayedValues[key] = target;
                        continue;
                    }

                    if (Mathf.Approximately(displayed, target)) continue;

                    _displayedValues[key] = Mathf.MoveTowards(displayed, target, AnimationSpeed * deltaTime);
                }
            }

            // cleanup removed stats
            var existingKeys = new HashSet<string>();
            var existingStats = _statManager?.GetStats();
            if (existingStats != null)
            {
                foreach (var s in existingStats)
                {
                    if (s == null) continue;
                    existingKeys.Add(s.Stat.ToString());
                }
            }

            var toRemove = new List<string>();
            foreach (var k in _displayedValues.Keys)
            {
                if (!existingKeys.Contains(k)) toRemove.Add(k);
            }

            foreach (var k in toRemove) _displayedValues.Remove(k);
        }

        public float GetDisplayedValue(GameStat stat)
        {
            if (stat == null) return 0f;
            if (_displayedValues.TryGetValue(stat.Stat.ToString(), out var v)) return v;
            return stat.CurrentValue;
        }
    }
}