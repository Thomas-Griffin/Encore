using Encore.Model.Game;
using Encore.Systems.Core;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameManagerTests
    {
        private GameManager _gameManager;
        private GameObject _gameObject;

        [OneTimeSetUp]
        public void Setup()
        {
            _gameObject = new GameObject("GameManagerTestObject");
            _gameManager = _gameObject.AddComponent<GameManager>();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_gameObject);
        }

        [Test]
        public void StartGame_Initialises_GameState_And_Stats()
        {
            _gameManager.StartGame(DifficultyLevel.Easy);
            Assert.AreEqual(GameState.Playing, _gameManager.Instance.State,
                "Game state should be Playing after starting the game");
            Assert.AreEqual(_gameManager.Instance.Stats.Energy.MaxValue,
                _gameManager.Instance.Stats.Energy.CurrentValue,
                "Energy should be initialized to full on Easy difficulty");
            Assert.AreEqual(0, _gameManager.Instance.Stats.Skill.CurrentValue, "Skill should be initialized to 0");
            Assert.AreEqual(0, _gameManager.Instance.Stats.Popularity.CurrentValue,
                "Popularity should be initialized to 0");
            Assert.AreEqual(0, _gameManager.Instance.Stats.Fame.CurrentValue, "Fame should be initialized to 0");
        }

        [Test]
        public void Rest_Increases_Energy_Decreases_Popularity()
        {
            _gameManager.Instance.Stats.Energy.CurrentValue = _gameManager.Instance.Stats.Energy.MinValue;
            _gameManager.Instance.Stats.Popularity.CurrentValue = 50;

            int startEnergy = _gameManager.Instance.Stats.Energy.CurrentValue;
            int startPopularity = _gameManager.Instance.Stats.Popularity.CurrentValue;

            _gameManager.DoAction(new SimpleGameAction(GameActions.Rest));

            Assert.Greater(_gameManager.Instance.Stats.Energy.CurrentValue, startEnergy,
                "Energy should increase after resting");
            Assert.Less(_gameManager.Instance.Stats.Popularity.CurrentValue, startPopularity,
                "Popularity should decrease after resting");
        }

        [Test]
        public void Gig_Decreases_Energy_Increases_Popularity()
        {
            _gameManager.Instance.Stats.Energy.CurrentValue =
                _gameManager.Instance.Stats.Energy.MaxValue; // Ensure enough energy for gig
            _gameManager.Instance.Stats.Popularity.CurrentValue =
                _gameManager.Instance.Stats.Popularity.MinValue; // Set popularity to a known state
            _gameManager.Instance.Stats.Skill.CurrentValue =
                _gameManager.Instance.Stats.Skill.MaxValue; // Ensure skill is high enough for gig

            int startEnergy = _gameManager.Instance.Stats.Energy.CurrentValue;
            int startPopularity = _gameManager.Instance.Stats.Popularity.CurrentValue;

            _gameManager.DoAction(new SimpleGameAction(GameActions.Gig));

            Assert.Less(_gameManager.Instance.Stats.Energy.CurrentValue, startEnergy,
                "Energy should decrease after a gig");
            Assert.Greater(_gameManager.Instance.Stats.Popularity.CurrentValue, startPopularity,
                "Popularity should increase after a gig");
        }

        [Test]
        public void Practice_Decreases_Energy_Increases_Skill()
        {
            _gameManager.Instance.Stats.Energy.CurrentValue =
                _gameManager.Instance.Stats.Energy.MaxValue; // Ensure enough energy for practice
            _gameManager.Instance.Stats.Skill.CurrentValue =
                _gameManager.Instance.Stats.Skill.MinValue; // Set skill to a known state

            int startEnergy = _gameManager.Instance.Stats.Energy.CurrentValue;
            int startSkill = _gameManager.Instance.Stats.Skill.CurrentValue;

            _gameManager.DoAction(new SimpleGameAction(GameActions.Practice));

            Assert.Less(_gameManager.Instance.Stats.Energy.CurrentValue, startEnergy,
                "Energy should decrease after practice");
            Assert.Greater(_gameManager.Instance.Stats.Skill.CurrentValue, startSkill,
                "Skill should increase after practice");
        }

        [Test]
        public void Game_Can_Be_Won()
        {
            _gameManager.StartGame(DifficultyLevel.Easy);
            _gameManager.Instance.Stats.Fame.CurrentValue = _gameManager.Instance.Stats.Fame.MaxValue;

            _gameManager.CheckForEndGameConditions();

            Assert.AreEqual(GameState.Win, _gameManager.Instance.State, "Game should be won when fame is maxed");
        }

        [Test]
        public void Game_Can_Be_Lost()
        {
            _gameManager.StartGame(DifficultyLevel.Easy);
            _gameManager.Instance.Stats.Energy.CurrentValue = _gameManager.Instance.Stats.Energy.MinValue;
            _gameManager.CheckForEndGameConditions();
            Assert.AreEqual(GameState.Lose, _gameManager.Instance.State,
                "Game should be lost when energy is depleted");
        }
    }
}