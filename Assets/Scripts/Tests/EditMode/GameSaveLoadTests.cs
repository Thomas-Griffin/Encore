using Encore.Abstractions.Interfaces;
using Encore.Model.Player.Actions;
using Encore.Systems.Configurations;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using Encore.Systems.Save;
using NUnit.Framework;
using Tests.EditMode.Mocks;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameSaveLoadTests
    {
        private GameManager _game;
        private GameSession _session;
        private IEventStore _events;
        private IStatService _stats;
        private ISaveService _saveService;
        private IDayService _dayService;
        
        [SetUp]
        public void Setup()
        {
             _session = new GameSession();
             _events = new EventStore();
             _stats = new StatsService(ScriptableObject.CreateInstance<StatsConfig>());
             _saveService = new InMemorySaveService();
             _dayService = new DayService();

            _game = new GameManager(
                _session,
                _events,
                _stats,
                _saveService,
                _dayService
            );

            _game.StartGame();
        }
        
        [Test]
        public void SaveAndLoad_Roundtrip_PreservesStatsAndEvents()
        {
            _game.DoAction(new Practice());
            _game.DoAction(new Rest());
            _game.DoAction(new Gig());

            int eventsBefore = _events.Count;
            int energyBefore = _stats.Energy.CurrentValue;
            int skillBefore = _stats.Skill.CurrentValue;
            int popularityBefore = _stats.Popularity.CurrentValue;
            int fameBefore = _stats.Fame.CurrentValue;


            SavedGame loadedSession = _saveService.Load();
            
            Assert.NotNull(loadedSession);
            Assert.AreEqual(_session.PlayState, loadedSession.ToGame().PlayState);
            Assert.AreEqual(_session.Difficulty, loadedSession.ToGame().Difficulty);
            Assert.AreEqual(_dayService.CurrentDay, loadedSession.saveData.daysCurrent);
            Assert.AreEqual(_dayService.TotalDays, loadedSession.saveData.daysTotal);

            Assert.AreEqual(eventsBefore, loadedSession.saveData.events.Count);

            IStatService loadedStats = loadedSession.GetStats();
            Assert.NotNull(loadedStats);
            
            Assert.AreEqual(energyBefore, loadedStats.Energy.CurrentValue);
            Assert.AreEqual(skillBefore, loadedStats.Skill.CurrentValue);
            Assert.AreEqual(popularityBefore, loadedStats.Popularity.CurrentValue);
            Assert.AreEqual(fameBefore, loadedStats.Fame.CurrentValue);
        }
    }
}