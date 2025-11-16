using System;
using System.Collections.Generic;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.GameEvent;
using UnityEngine;

namespace Encore.Systems.Core
{
    [CreateAssetMenu(menuName = "Encore/StatManager")]
    public class StatManager : ScriptableObject
    {
        private int FameTarget { get; set; }

        // Skill requirement to gig
        public int SkillBarrierForGig { get; private set; }

        private GameStat _energy;
        private GameStat _skill;
        private GameStat _popularity;
        private GameStat _fame;

        private GameStat[] _cachedStats;

        public GameStat Energy
        {
            get
            {
                EnsureInitialised();
                return _energy;
            }
            private set => _energy = value;
        }

        public GameStat Skill
        {
            get
            {
                EnsureInitialised();
                return _skill;
            }
            private set => _skill = value;
        }

        public GameStat Popularity
        {
            get
            {
                EnsureInitialised();
                return _popularity;
            }
            private set => _popularity = value;
        }

        public GameStat Fame
        {
            get
            {
                EnsureInitialised();
                return _fame;
            }
            private set => _fame = value;
        }

        public DifficultyLevel Difficulty { get; private set; }

        private bool _isInitialized;

        public StatManager()
        {
        }

        public StatManager(DifficultyLevel difficulty)
        {
            InitialiseDefaults(difficulty);
            _isInitialized = true;
        }

        private void OnEnable()
        {
            if (_energy == null || _skill == null || _popularity == null || _fame == null)
            {
                InitialiseDefaults(DifficultyLevel.Easy);
            }

            _isInitialized = true;
        }

        private void EnsureInitialised()
        {
            if (_isInitialized) return;
            if (_energy == null || _skill == null || _popularity == null || _fame == null)
            {
                InitialiseDefaults(DifficultyLevel.Easy);
            }

            _isInitialized = true;
        }

        private void InitialiseDefaults(DifficultyLevel difficulty)
        {
            Difficulty = difficulty;
            FameTarget = difficulty switch
            {
                DifficultyLevel.Easy => 100,
                DifficultyLevel.Medium => 200,
                DifficultyLevel.Hard => 300,
                _ => 100
            };
            _energy = new GameStat(
                stat: GameStats.Energy,
                colour: Color.green,
                initialValue: 100,
                minValue: 0,
                maxValue: 100,
                maximumIncrease: 60,
                maximumDecrease: 60,
                minimumIncrease: 1,
                minimumDecrease: 1
            );
            _skill = new GameStat(
                stat: GameStats.Skill,
                colour: Color.orange,
                initialValue: 0,
                minValue: 0,
                maxValue: 10,
                maximumIncrease: 10,
                maximumDecrease: 10,
                minimumIncrease: 1,
                minimumDecrease: 1
            );
            _popularity = new GameStat(
                stat: GameStats.Popularity,
                colour: Color.blue,
                initialValue: 0,
                minValue: 0,
                maxValue: 50,
                maximumIncrease: 5,
                maximumDecrease: 10,
                minimumIncrease: 1,
                minimumDecrease: 1
            );

            _fame = new GameStat(
                stat: GameStats.Fame,
                colour: Color.purple,
                initialValue: 0,
                minValue: 0,
                maxValue: FameTarget,
                maximumIncrease: 20,
                maximumDecrease: 20,
                minimumIncrease: 1,
                minimumDecrease: 1
            );

            _cachedStats = new[] { _energy, _skill, _popularity, _fame };
        }

        public void InitialiseStats(DifficultyLevel difficulty)
        {
            EnsureInitialised();
            int initialEnergy = difficulty switch
            {
                DifficultyLevel.Easy => 100,
                DifficultyLevel.Medium => 80,
                DifficultyLevel.Hard => 60,
                _ => 100
            };
            _energy.InitialValue = initialEnergy;
            _energy.CurrentValue = initialEnergy;
            _skill.CurrentValue = 0;
            _popularity.CurrentValue = 0;
            _fame.CurrentValue = 0;
            // Ensure the cached array reflects any changes to the stat instances
            _cachedStats = new[] { _energy, _skill, _popularity, _fame };
            FameTarget = difficulty switch
            {
                DifficultyLevel.Easy => 50,
                DifficultyLevel.Medium => 100,
                DifficultyLevel.Hard => 200,
                _ => 50
            };
        }

        public void IncreaseStat(GameStats stat)
        {
            EnsureInitialised();
            GetStat(stat)?.Increase();
        }

        public void DecreaseStat(GameStats stat)
        {
            EnsureInitialised();
            GetStat(stat)?.Decrease();
        }

        public void IncreaseStatBy(GameStats stat, int amount)
        {
            EnsureInitialised();
            GetStat(stat)?.IncreaseBy(amount);
        }

        public void DecreaseStatBy(GameStats stat, int amount)
        {
            EnsureInitialised();
            GetStat(stat)?.DecreaseBy(amount);
        }

        private GameStat GetStat(GameStats stat)
        {
            return stat switch
            {
                GameStats.Energy => _energy,
                GameStats.Skill => _skill,
                GameStats.Popularity => _popularity,
                GameStats.Fame => _fame,
                _ => null
            };
        }

        public GameStat[] GetStats()
        {
            EnsureInitialised();
            if (_cachedStats is { Length: > 0 })
            {
                foreach (GameStat gameStat in _cachedStats)
                {
                    if (gameStat == null) goto Rebuild;
                }

                return _cachedStats;
            }

            Rebuild:
            List<GameStat> list = new();
            if (_energy != null) list.Add(_energy);
            if (_skill != null) list.Add(_skill);
            if (_popularity != null) list.Add(_popularity);
            if (_fame != null) list.Add(_fame);
            _cachedStats = list.ToArray();
            return _cachedStats;
        }

        public bool LoseConditionMet()
        {
            EnsureInitialised();
            return _energy != null && _energy.IsAtMinimum();
        }

        public bool WinConditionMet()
        {
            EnsureInitialised();
            return _fame != null && _fame.IsAtMaximum();
        }

        public void ResetStats()
        {
            EnsureInitialised();
            GameStat[] stats = GetStats();
            if (stats == null || stats.Length == 0) return;

            foreach (GameStat stat in stats)
            {
                if (stat == null) continue;
                try
                {
                    stat.Reset();
                }
                catch (Exception exception)
                {
                    Debug.LogError(
                        $"StatManager.ResetStats: Failed to reset stat '{stat.Stat.ToString() ?? "<null>"}' - {exception}");
                }
            }
        }

        private void ApplyDeltaToStat(GameStats stat, int delta)
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

        public void ApplyDeltas(StatDeltas deltas)
        {
            EnsureInitialised();
            if (deltas == null) return;
            ApplyDeltaToStat(GameStats.Energy, deltas.energyDelta);
            ApplyDeltaToStat(GameStats.Skill, deltas.skillDelta);
            ApplyDeltaToStat(GameStats.Popularity, deltas.popularityDelta);
            ApplyDeltaToStat(GameStats.Fame, deltas.fameDelta);
        }
    }
}