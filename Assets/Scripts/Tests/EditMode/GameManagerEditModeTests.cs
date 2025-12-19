using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Model.Player.Actions;
using Encore.Systems.Configurations;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using NUnit.Framework;
using Tests.EditMode.Mocks;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameManagerTests
    {
        private GameManager _game;
        private GameSession _session;
        private IEventStore _events;
        private IStatService _stats;
        private ISaveService _save;
        private IDayService _days;

        [SetUp]
        public void Setup()
        {
            _session = new GameSession();
            _events = new EventStore();
            _save = new MockSaveService();
            _days = new DayService();
            _stats = new StatsService(ScriptableObject.CreateInstance<StatsConfig>());
            _stats.InitialiseStats(Difficulty.Easy);
            _game = new GameManager(_session, _events, _stats, _save, _days);
        }

        [Test]
        public void StartGame_Initialises_GameState_And_Stats()
        {
            _game.StartGame();

            Assert.AreEqual(PlayState.Playing, _session.PlayState);
            Assert.AreEqual(_stats.Energy.MaxValue, _stats.Energy.CurrentValue);
            Assert.AreEqual(0, _stats.Skill.CurrentValue);
            Assert.AreEqual(0, _stats.Popularity.CurrentValue);
            Assert.AreEqual(0, _stats.Fame.CurrentValue);

            Assert.AreEqual(1, _days.CurrentDay);
            Assert.Greater(_days.TotalDays, 0);
        }

        [Test]
        public void Rest_Increases_Energy_Decreases_Popularity()
        {
            _game.StartGame();

            _stats.Energy.CurrentValue = _stats.Energy.MaxValue / 2;
            _stats.Popularity.CurrentValue = _stats.Popularity.MaxValue / 2;

            int startEnergy = _stats.Energy.CurrentValue;
            int startPopularity = _stats.Popularity.CurrentValue;

            _game.DoAction(new Rest());

            Assert.Greater(_stats.Energy.CurrentValue, startEnergy);
            Assert.Less(_stats.Popularity.CurrentValue, startPopularity);
        }

        [Test]
        public void Gig_Decreases_Energy_Increases_Popularity()
        {
            _game.StartGame();

            _stats.Energy.CurrentValue = _stats.Energy.MaxValue;
            _stats.Popularity.CurrentValue = _stats.Popularity.MinValue;
            _stats.Skill.CurrentValue = _stats.Skill.MaxValue;

            int startEnergy = _stats.Energy.CurrentValue;
            int startPopularity = _stats.Popularity.CurrentValue;

            _game.DoAction(new Gig());

            Assert.Less(_stats.Energy.CurrentValue, startEnergy);
            Assert.Greater(_stats.Popularity.CurrentValue, startPopularity);
        }

        [Test]
        public void Practice_Decreases_Energy_Increases_Skill()
        {
            _game.StartGame();

            _stats.Energy.CurrentValue = _stats.Energy.MaxValue;
            _stats.Skill.CurrentValue = _stats.Skill.MinValue;

            int startEnergy = _stats.Energy.CurrentValue;
            int startSkill = _stats.Skill.CurrentValue;

            _game.DoAction(new Practice());

            Assert.Less(_stats.Energy.CurrentValue, startEnergy);
            Assert.Greater(_stats.Skill.CurrentValue, startSkill);
        }

        [Test]
        public void Game_Can_Be_Won()
        {
            _game.StartGame();

            _stats.Fame.CurrentValue = _stats.Fame.MaxValue;

            _game.CheckForEndGameConditions();

            Assert.AreEqual(PlayState.Win, _session.PlayState);
            Assert.Contains(WinReasons.AchievedFameTarget, _session.WinReasons);
        }

        [Test]
        public void Game_Can_Be_Lost()
        {
            _game.StartGame();

            _stats.Energy.CurrentValue = _stats.Energy.MinValue;

            _game.CheckForEndGameConditions();

            Assert.AreEqual(PlayState.Lose, _session.PlayState);
            Assert.Contains(LoseReasons.EnergyDepleted, _session.LoseReasons);
        }
    }
}