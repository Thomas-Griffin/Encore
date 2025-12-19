using System;
using System.Collections.Generic;
using Encore.Abstractions.Interfaces;
using Encore.Model.Game;
using Encore.Model.Player;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;

namespace Encore.Systems.Core
{
    public sealed class GameManager
    {
        private readonly GameSession _session;
        private readonly IEventStore _events;
        private readonly IStatService _stats;
        private readonly ISaveService _saveService;
        private readonly IDayService _dayService;
        private SavedGame _save;

        public GameManager(
            GameSession session,
            IEventStore events,
            IStatService stats,
            ISaveService saveService,
            IDayService dayService)
        {
            _session = session;
            _events = events;
            _stats = stats;
            _saveService = saveService;
            _dayService = dayService;
        }

        public void StartGame(Difficulty difficulty = Difficulty.Easy)
        {
            _session.PlayState = PlayState.Playing;
            _session.Difficulty = difficulty;

            _session.LoseReasons ??= new List<LoseReasons>();
            _session.WinReasons ??= new List<WinReasons>();
            _session.LoseReasons.Clear();
            _session.WinReasons.Clear();

            _dayService.Initialise(difficulty);
            _stats.InitialiseStats(difficulty);

            _events.Clear();
            _events.Append(new StartGameEvent());

            SaveGame();
        }


        private void SaveGame()
        {
            _save = new SavedGame(
                _session,
                _stats,
                _dayService,
                _events,
                DateTime.UtcNow
            );
            _saveService.Save(_save, _events.EventsAsSnapshots());
        }

        public void DoAction(PlayerAction action)
        {
            RecordApplyAndAppend(action.ToGameEvent());
            RecordApplyAndAppend(new NextDayEvent());

            CheckForEndGameConditions();

            SaveGame();
        }

        private void RecordApplyAndAppend(GameEventBase gameEvent)
        {
            while (true)
            {
                
                _events.Append(StampConsecutiveRepetitions(gameEvent).Apply(_session, _stats, _dayService));

                if (gameEvent.DelegateEvent != null)
                {
                    gameEvent = gameEvent.DelegateEvent;
                    continue;
                }

                break;
            }
        }

        private GameEventBase StampConsecutiveRepetitions(GameEventBase gameEvent)
        {
            if (gameEvent?.Action == null)
            {
                if (gameEvent != null) gameEvent.ConsecutiveEventRepetitions = 1;
                return gameEvent;
            }

            GameEventBase lastActionEvent = _events.GetLastActionEvent();

            if (lastActionEvent?.Action != null &&
                lastActionEvent.Action.GetType() == gameEvent.Action.GetType())
            {
                gameEvent.ConsecutiveEventRepetitions = lastActionEvent.ConsecutiveEventRepetitions + 1;
            }
            else
            {
                gameEvent.ConsecutiveEventRepetitions = 1;
            }

            return gameEvent;
        }

        public void CheckForEndGameConditions()
        {
            CheckForWinConditions();
            CheckForLoseConditions();
        }

        private void CheckForLoseConditions()
        {
            _session.LoseReasons.Clear();

            if (_stats.LoseConditionMet())
            {
                _session.LoseReasons.Add(LoseReasons.EnergyDepleted);
                _session.PlayState = PlayState.Lose;
                return;
            }

            if (_dayService.CurrentDay != _dayService.TotalDays) return;
            if (_stats.WinConditionMet()) return;
            _session.LoseReasons.Add(LoseReasons.RanOutOfTime);
            _session.PlayState = PlayState.Lose;
        }

        private void CheckForWinConditions()
        {
            _session.WinReasons.Clear();

            if (!_stats.WinConditionMet()) return;
            _session.WinReasons.Add(WinReasons.AchievedFameTarget);
            _session.PlayState = PlayState.Win;
        }
    }
}