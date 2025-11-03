using NUnit.Framework;

namespace Tests.EditMode
{
    public class StatManagerEditModeTests
    {
        private StatManager _statManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _statManager = new StatManager();
            _statManager.InitialiseStats(DifficultyLevel.Easy);
        }

        [Test]
        public void IncreaseStat_Increases_Correct_Stat_Value()
        {
            int initialSkill = _statManager.Skill.CurrentValue;
            _statManager.IncreaseStat(GameStats.Skill);
            Assert.Greater(_statManager.Skill.CurrentValue, initialSkill,
                "Skill stat should increase after calling IncreaseStat on it");
        }

        [Test]
        public void DecreaseStat_Decreases_Correct_Stat_Value()
        {
            _statManager.Energy.CurrentValue = _statManager.Energy.MaxValue; // Ensure enough energy
            int initialEnergy = _statManager.Energy.CurrentValue;
            _statManager.DecreaseStat(GameStats.Energy);
            Assert.Less(_statManager.Energy.CurrentValue, initialEnergy,
                "Energy stat should decrease after calling DecreaseStat");
        }

        [Test]
        public void LoseConditionMet_Returns_True_When_Energy_At_Minimum()
        {
            _statManager.Energy.CurrentValue = _statManager.Energy.MinValue;
            Assert.IsTrue(_statManager.LoseConditionMet(), "Lose condition should be met when energy is at minimum");
        }

        [Test]
        public void WinConditionMet_Returns_True_When_Fame_At_Maximum()
        {
            _statManager.Fame.CurrentValue = _statManager.Fame.MaxValue;
            Assert.IsTrue(_statManager.WinConditionMet(), "Win condition should be met when fame is at maximum");
        }

        [Test]
        public void ResetStats_Sets_All_Stats_To_Initial_Values()
        {
            _statManager.Energy.CurrentValue = 50;
            _statManager.Skill.CurrentValue = 20;
            _statManager.Popularity.CurrentValue = 30;
            _statManager.Fame.CurrentValue = 40;
            _statManager.ResetStats();
            Assert.AreEqual(_statManager.Energy.CurrentValue, _statManager.Energy.InitialValue,
                "Energy should be reset to minimum value");
            Assert.AreEqual(_statManager.Skill.CurrentValue, _statManager.Skill.InitialValue,
                "Skill should be reset to minimum value");
            Assert.AreEqual(_statManager.Popularity.CurrentValue, _statManager.Popularity.InitialValue,
                "Popularity should be reset to minimum value");
            Assert.AreEqual(_statManager.Fame.CurrentValue, _statManager.Fame.InitialValue,
                "Fame should be reset to minimum value");
        }
    }
}