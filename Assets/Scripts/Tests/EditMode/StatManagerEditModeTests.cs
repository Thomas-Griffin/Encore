using System.IO;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class StatManagerEditModeTests
    {
        private StatManager _statManager;
        private SaveManager _saveManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _statManager = ScriptableObject.CreateInstance<StatManager>();
            _statManager.InitialiseStats(DifficultyLevel.Easy);
        }
        
        public void BeforeEach()
        {
            _statManager.ResetStats();
            _saveManager.DeleteAllSaves();
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_statManager);
        }

        [Test]
        public void IncreaseStat_Increases_Correct_Stat_Value()
        {
            int initialSkill = _statManager.Skill.CurrentValue;
            _statManager.IncreaseStat(GameStats.Skill);
            Assert.Greater(_statManager.Skill.CurrentValue, initialSkill);
        }

        [Test]
        public void DecreaseStat_Decreases_Correct_Stat_Value()
        {
            _statManager.Energy.CurrentValue = _statManager.Energy.MaxValue;
            int initialEnergy = _statManager.Energy.CurrentValue;
            _statManager.DecreaseStat(GameStats.Energy);
            Assert.Less(_statManager.Energy.CurrentValue, initialEnergy);
        }

        [Test]
        public void LoseConditionMet_Returns_True_When_Energy_At_Minimum()
        {
            _statManager.Energy.CurrentValue = _statManager.Energy.MinValue;
            Assert.IsTrue(_statManager.LoseConditionMet());
        }

        [Test]
        public void WinConditionMet_Returns_True_When_Fame_At_Maximum()
        {
            _statManager.Fame.CurrentValue = _statManager.Fame.MaxValue;
            Assert.IsTrue(_statManager.WinConditionMet());
        }

        [Test]
        public void ResetStats_Sets_All_Stats_To_Initial_Values()
        {
            _statManager.Energy.CurrentValue = 50;
            _statManager.Skill.CurrentValue = 20;
            _statManager.Popularity.CurrentValue = 30;
            _statManager.Fame.CurrentValue = 40;
            _statManager.ResetStats();
            Assert.AreEqual(_statManager.Energy.CurrentValue, _statManager.Energy.InitialValue);
            Assert.AreEqual(_statManager.Skill.CurrentValue, _statManager.Skill.InitialValue);
            Assert.AreEqual(_statManager.Popularity.CurrentValue, _statManager.Popularity.InitialValue);
            Assert.AreEqual(_statManager.Fame.CurrentValue, _statManager.Fame.InitialValue);
        }

        [Test]
        public void IncreaseStatBy_Adds_Specified_Amount()
        {
            int initial = _statManager.Skill.CurrentValue;
            _statManager.IncreaseStatBy(GameStats.Skill, 3);
            Assert.AreEqual(initial + 3, _statManager.Skill.CurrentValue);
        }

        [Test]
        public void DecreaseStatBy_Subtracts_Specified_Amount()
        {
            _statManager.Energy.CurrentValue = _statManager.Energy.MaxValue;
            int initial = _statManager.Energy.CurrentValue;
            _statManager.DecreaseStatBy(GameStats.Energy, 15);
            Assert.AreEqual(initial - 15, _statManager.Energy.CurrentValue);
        }

        [Test]
        public void ApplyDeltas_Applies_All_Deltas_To_Stats()
        {
            _statManager.InitialiseStats(DifficultyLevel.Easy);
            StatDeltas deltas = new()
            {
                energyDelta = -10,
                skillDelta = 2,
                popularityDelta = 5,
                fameDelta = 1
            };

            int energyBefore = _statManager.Energy.CurrentValue;
            int skillBefore = _statManager.Skill.CurrentValue;
            int popBefore = _statManager.Popularity.CurrentValue;
            int fameBefore = _statManager.Fame.CurrentValue;

            _statManager.ApplyDeltas(deltas);

            Assert.AreEqual(energyBefore - 10, _statManager.Energy.CurrentValue);
            Assert.AreEqual(skillBefore + 2, _statManager.Skill.CurrentValue);
            Assert.AreEqual(popBefore + 5, _statManager.Popularity.CurrentValue);
            Assert.AreEqual(fameBefore + 1, _statManager.Fame.CurrentValue);
        }

        [Test]
        public void ConsecutivePractise_Compounds_SkillGain()
        {
            const string saveName = "test_consecutive_practice";

            try
            {
                string preSaveDir = Path.Combine(Application.persistentDataPath, "Saves");
                string preSaveFile = Path.Combine(preSaveDir, saveName + ".jsonl");
                if (File.Exists(preSaveFile)) File.Delete(preSaveFile);
            }
            catch
            {
                // ignored
            }

            GameInstance instance = new(saveName, DifficultyLevel.Easy);
            instance.Stats.InitialiseStats(DifficultyLevel.Easy);

            try
            {
                SimpleGameAction practiceActionFirst = new(GameActions.Practice);
                GameEventBase practiceEventFirst = practiceActionFirst.ToGameEvent();
                instance.Events.Append(instance.ApplyEvent(practiceEventFirst));
                int skillAfterFirstPractice = instance.Stats.Skill.CurrentValue;

                SimpleGameAction practiceActionSecond = new(GameActions.Practice);
                GameEventBase practiceEventSecond = practiceActionSecond.ToGameEvent();
                instance.Events.Append(instance.ApplyEvent(practiceEventSecond));
                int skillAfterSecondPractice = instance.Stats.Skill.CurrentValue;

                SimpleGameAction practiceActionThird = new(GameActions.Practice);
                GameEventBase practiceEventThird = practiceActionThird.ToGameEvent();
                instance.Events.Append(instance.ApplyEvent(practiceEventThird));
                int skillAfterThirdPractice = instance.Stats.Skill.CurrentValue;

                Assert.AreEqual(1, skillAfterFirstPractice);
                Assert.AreEqual(1 + 2, skillAfterSecondPractice);
                Assert.AreEqual(1 + 2 + 3, skillAfterThirdPractice);
            }
            finally
            {
                try
                {
                    string saveDir = Path.Combine(Application.persistentDataPath, "Saves");
                    string saveFile = Path.Combine(saveDir, saveName + ".jsonl");
                    if (File.Exists(saveFile)) File.Delete(saveFile);
                }
                catch
                {
                    // ignored
                }
            }
        }

        [Test]
        public void Repetitions_Reset_After_Different_Action()
        {
            const string saveName = "test_repetitions_reset";

            try
            {
                string preSaveDir = Path.Combine(Application.persistentDataPath, "Saves");
                string preSaveFile = Path.Combine(preSaveDir, saveName + ".jsonl");
                if (File.Exists(preSaveFile)) File.Delete(preSaveFile);
            }
            catch
            {
                // ignored
            }

            GameInstance instance = new(saveName, DifficultyLevel.Easy);
            instance.Stats.InitialiseStats(DifficultyLevel.Easy);

            try
            {
                SimpleGameAction firstPracticeAction = new(GameActions.Practice);
                instance.Events.Append(instance.ApplyEvent(firstPracticeAction.ToGameEvent()));
                int skillAfterFirstPractice = instance.Stats.Skill.CurrentValue;

                SimpleGameAction secondPracticeAction = new(GameActions.Practice);
                instance.Events.Append(instance.ApplyEvent(secondPracticeAction.ToGameEvent()));
                int skillAfterSecondPractice = instance.Stats.Skill.CurrentValue;

                SimpleGameAction gigAction = new(GameActions.Gig);
                instance.Events.Append(instance.ApplyEvent(gigAction.ToGameEvent()));
                int skillAfterGig = instance.Stats.Skill.CurrentValue;

                SimpleGameAction thirdPracticeAction = new(GameActions.Practice);
                instance.Events.Append(instance.ApplyEvent(thirdPracticeAction.ToGameEvent()));
                int skillAfterPracticeAfterGig = instance.Stats.Skill.CurrentValue;

                Assert.GreaterOrEqual(skillAfterFirstPractice, 1);
                Assert.Greater(skillAfterSecondPractice, skillAfterFirstPractice);
                Assert.AreEqual(skillAfterGig, skillAfterSecondPractice);
                Assert.AreEqual(skillAfterGig + 1, skillAfterPracticeAfterGig);
            }
            finally
            {
                try
                {
                    string saveDir = Path.Combine(Application.persistentDataPath, "Saves");
                    string saveFile = Path.Combine(saveDir, saveName + ".jsonl");
                    if (File.Exists(saveFile)) File.Delete(saveFile);
                }
                catch
                {
                    // ignored
                }
            }
        }

    }
}