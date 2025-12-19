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
    public class ActionRepetitionTests
    {
        private GameSession _session;
        private IEventStore _events;
        private IStatService _stats;
        private ISaveService _save;
        private GameManager _game;
        private IDayService _dayService;

        [SetUp]
        public void Setup()
        {
            _session = new GameSession();
            _events = new EventStore();
            _stats = new StatsService(ScriptableObject.CreateInstance<StatsConfig>());
            _save = new MockSaveService();
            _dayService = new DayService();
            _game = new GameManager(_session, _events, _stats, _save, _dayService);
            _game.StartGame();
        }

        [Test]
        public void ConsecutivePractise_Compounds_SkillGain()
        {
            _game.DoAction(new Practice());
            int skillAfterFirst = _stats.Skill.CurrentValue;

            _game.DoAction(new Practice());
            int skillAfterSecond = _stats.Skill.CurrentValue;

            _game.DoAction(new Practice());
            int skillAfterThird = _stats.Skill.CurrentValue;

            Assert.AreEqual(1, skillAfterFirst);
            Assert.AreEqual(1 + 2, skillAfterSecond);
            Assert.AreEqual(1 + 2 + 3, skillAfterThird);
        }

        [Test]
        public void Repetitions_Reset_After_Different_Action()
        {
            _game.DoAction(new Practice());
            int skillAfterFirst = _stats.Skill.CurrentValue;

            _game.DoAction(new Practice());
            int skillAfterSecond = _stats.Skill.CurrentValue;

            _game.DoAction(new Gig());
            int skillAfterGig = _stats.Skill.CurrentValue;

            _game.DoAction(new Practice());
            int skillAfterPracticeAfterGig = _stats.Skill.CurrentValue;

            Assert.GreaterOrEqual(skillAfterFirst, 1);
            Assert.Greater(skillAfterSecond, skillAfterFirst);

            Assert.AreEqual(skillAfterGig, skillAfterSecond);

            Assert.AreEqual(skillAfterGig + 1, skillAfterPracticeAfterGig);
        }
    }
}