using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Systems.Configurations;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class StatsServiceTests
    {
        private IStatService _stats;

        [SetUp]
        public void Setup()
        {
            _stats = new StatsService(ScriptableObject.CreateInstance<StatsConfig>());
            _stats.InitialiseStats(Difficulty.Easy);
        }

        [Test]
        public void LoseConditionMet_Returns_True_When_Energy_At_Minimum()
        {
            _stats.Energy.CurrentValue = _stats.Energy.MinValue;
            Assert.IsTrue(_stats.LoseConditionMet());
        }

        [Test]
        public void WinConditionMet_Returns_True_When_Fame_At_Maximum()
        {
            _stats.Fame.CurrentValue = _stats.Fame.MaxValue;
            Assert.IsTrue(_stats.WinConditionMet());
        }

        [Test]
        public void ApplyDeltas_Applies_All_Deltas_To_Stats()
        {
            StatDeltas deltas = new()
            {
                energyDelta = -10,
                skillDelta = 2,
                popularityDelta = 5,
                fameDelta = 1
            };

            int energyBefore = _stats.Energy.CurrentValue;
            int skillBefore = _stats.Skill.CurrentValue;
            int popBefore = _stats.Popularity.CurrentValue;
            int fameBefore = _stats.Fame.CurrentValue;

            _stats.ApplyDeltas(deltas);

            Assert.AreEqual(energyBefore - 10, _stats.Energy.CurrentValue);
            Assert.AreEqual(skillBefore + 2, _stats.Skill.CurrentValue);
            Assert.AreEqual(popBefore + 5, _stats.Popularity.CurrentValue);
            Assert.AreEqual(fameBefore + 1, _stats.Fame.CurrentValue);
        }
    }
}