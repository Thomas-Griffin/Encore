using Encore.Abstractions.Interfaces;
using Encore.Systems.Configurations;
using Encore.Systems.GameEvent;
using Encore.Systems.Save;
using UnityEngine;

namespace Encore.Systems.Core
{
    public sealed class GameContext : MonoBehaviour, IGameContext
    {
        public static GameContext Instance { get; private set; }

        public GameManager GameManager { get; private set; }
        public GameSession Session { get; private set; }
        public IStatService Stats { get; private set; }
        public IEventStore Events { get; private set; }
        public IDayService DayService { get; private set; }
        public ISaveService SaveService { get; private set; }


        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Events = new EventStore();
            Stats = new StatsService(ScriptableObject.CreateInstance<StatsConfig>());
            DayService = new DayService();
            SaveService = new JsonlFileSaveService();
            Session = new GameSession();

            GameManager = new GameManager(Session, Events, Stats, SaveService, DayService);
        }
    }
}