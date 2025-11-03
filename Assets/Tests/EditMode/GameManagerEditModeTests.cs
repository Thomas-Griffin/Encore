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
            _gameManager = new GameManager();
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
            Assert.AreEqual(GameState.Playing, _gameManager.CurrentGameState,
                "Game state should be Playing after starting the game");
            Assert.AreEqual(_gameManager.StatManager.Energy.MaxValue, _gameManager.StatManager.Energy.CurrentValue,
                "Energy should be initialized to full on Easy difficulty");
            Assert.AreEqual(0, _gameManager.StatManager.Skill.CurrentValue, "Skill should be initialized to 0");
            Assert.AreEqual(0, _gameManager.StatManager.Popularity.CurrentValue,
                "Popularity should be initialized to 0");
            Assert.AreEqual(0, _gameManager.StatManager.Fame.CurrentValue, "Fame should be initialized to 0");
        }

        [Test]
        public void Rest_Increases_Energy_Decreases_Popularity()
        {
            _gameManager.StatManager.Energy.CurrentValue = _gameManager.StatManager.Energy.MinValue;
            _gameManager.StatManager.Popularity.CurrentValue = 50;

            int startEnergy = _gameManager.StatManager.Energy.CurrentValue;
            int startPopularity = _gameManager.StatManager.Popularity.CurrentValue;

            _gameManager.DoRest();

            Assert.Greater(_gameManager.StatManager.Energy.CurrentValue, startEnergy,
                "Energy should increase after resting");
            Assert.Less(_gameManager.StatManager.Popularity.CurrentValue, startPopularity,
                "Popularity should decrease after resting");
        }

        [Test]
        public void Gig_Decreases_Energy_Increases_Popularity()
        {
            _gameManager.StatManager.Energy.CurrentValue =
                _gameManager.StatManager.Energy.MaxValue; // Ensure enough energy for gig
            _gameManager.StatManager.Popularity.CurrentValue =
                _gameManager.StatManager.Popularity.MinValue; // Set popularity to a known state

            int startEnergy = _gameManager.StatManager.Energy.CurrentValue;
            int startPopularity = _gameManager.StatManager.Popularity.CurrentValue;

            _gameManager.DoGig();

            Assert.Less(_gameManager.StatManager.Energy.CurrentValue, startEnergy,
                "Energy should decrease after a gig");
            Assert.Greater(_gameManager.StatManager.Popularity.CurrentValue, startPopularity,
                "Popularity should increase after a gig");
        }

        [Test]
        public void Practice_Decreases_Energy_Increases_Skill()
        {
            _gameManager.StatManager.Energy.CurrentValue =
                _gameManager.StatManager.Energy.MaxValue; // Ensure enough energy for practice
            _gameManager.StatManager.Skill.CurrentValue =
                _gameManager.StatManager.Skill.MinValue; // Set skill to a known state

            int startEnergy = _gameManager.StatManager.Energy.CurrentValue;
            int startSkill = _gameManager.StatManager.Skill.CurrentValue;

            _gameManager.DoPractice();

            Assert.Less(_gameManager.StatManager.Energy.CurrentValue, startEnergy,
                "Energy should decrease after practice");
            Assert.Greater(_gameManager.StatManager.Skill.CurrentValue, startSkill,
                "Skill should increase after practice");
        }

        [Test]
        public void Game_Can_Be_Won()
        {
            _gameManager.StartGame(DifficultyLevel.Easy);
            _gameManager.StatManager.Fame.CurrentValue = _gameManager.StatManager.Fame.MaxValue;

            _gameManager.CheckForEndGameConditions();

            Assert.AreEqual(GameState.Win, _gameManager.CurrentGameState, "Game should be won when fame is maxed");
        }

        [Test]
        public void Game_Can_Be_Lost()
        {
            _gameManager.StartGame(DifficultyLevel.Easy);
            _gameManager.StatManager.Energy.CurrentValue = _gameManager.StatManager.Energy.MinValue;
            _gameManager.CheckForEndGameConditions();
            Assert.AreEqual(GameState.Lose, _gameManager.CurrentGameState,
                "Game should be lost when energy is depleted");
        }
    }
}