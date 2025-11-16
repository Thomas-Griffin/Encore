using System;
using System.Collections.Generic;
using Encore.Model.Game;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;
using Encore.UI;
using UnityEngine;

namespace Encore.Systems.Core
{
    public class GameManager : MonoBehaviour
    {
        public SaveManager SaveManager { get; private set; }

        private GameInstance _instance;

        public GameInstance Instance
        {
            get
            {
                if (_instance == null)
                {
                    EnsureInitialised();
                }

                return _instance;
            }
            private set => _instance = value;
        }

        private bool _initialised;

        public GameManager()
        {
        }

        public GameManager(GameInstance instance)
        {
            _instance = instance;
        }

        private void Awake()
        {
            EnsureInitialised();
        }

        private void EnsureInitialised()
        {
            if (_initialised) return;

            SaveManager ??= new SaveManager();
            SaveManager.EnsureSaveDirectoryExists();

            _instance ??= new GameInstance();

            if (_instance?.Stats is null)
            {
                _instance!.Stats = ScriptableObject.CreateInstance<StatManager>();
            }

            _instance.Days ??= new DayManager(_instance.Difficulty);

            _initialised = true;
        }

        public void StartGame(DifficultyLevel difficulty)
        {
            EnsureInitialised();

            if (SaveManager.SaveFileExists(_instance?.SaveFileName))
            {
                _instance = SaveManager.LoadFromFile(_instance?.SaveFileName).ToGameInstance();
                return;
            }

            _instance ??= new GameInstance(DateTime.UtcNow.ToString("F"), difficulty);

            _instance.Difficulty = difficulty;
            _instance.Stats.InitialiseStats(difficulty);
            _instance.State = GameState.Playing;

            _instance.Events.Append(new StartGameEvent());
        }

        public void DoAction(GameAction action)
        {
            EnsureInitialised();

            GameEventBase actionAsEvent = action.ToGameEvent();

            RecordAndApplyEvent(actionAsEvent);

            RecordAndApplyEvent(new NextDayEvent());

            CheckForEndGameConditions();
            PerformAutoSave();
        }

        private void RecordAndApplyEvent(GameEventBase gameEvent)
        {
            while (true)
            {
                EnsureInitialised();

                _instance.Events.Append(_instance.ApplyEvent(gameEvent));
                if (gameEvent.DelegateEvent != null)
                {
                    gameEvent = gameEvent.DelegateEvent;
                    continue;
                }

                break;
            }
        }

        private void PerformAutoSave()
        {
            EnsureInitialised();

            string saveName = _instance.SaveFileName;
            if (string.IsNullOrWhiteSpace(saveName))
            {
                saveName = "autosave";
            }

            SaveManager.SaveToFile(new SaveData(saveName, 0, _instance, DateTime.UtcNow));
        }

        public void CheckForEndGameConditions()
        {
            EnsureInitialised();
            CheckForWinCondition();
            CheckForLoseCondition();

            // Show UI screens only if the UI manager exists (edit-mode tests may not create UI)
            UIScreenManager ui = UIScreenManager.Instance;
            switch (Instance?.State)
            {
                case GameState.Win:
                    ui?.ShowScreen(UIScreenNames.WinScreen);
                    break;
                case GameState.Lose:
                    ui?.ShowScreen(UIScreenNames.LoseScreen);
                    break;
                case GameState.Playing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void CheckForLoseCondition()
        {
            EnsureInitialised();
            _instance.LoseReasons = new List<LoseReasons>();
            if (_instance.Stats.LoseConditionMet())
            {
                _instance.LoseReasons.Add(LoseReasons.EnergyDepleted);
                _instance.State = GameState.Lose;
            }
            else if (_instance.Days.CurrentDay == _instance.Days.TotalDays)
            {
                if (_instance.Stats.WinConditionMet()) return;
                _instance.LoseReasons.Add(LoseReasons.RanOutOfTime);
                _instance.State = GameState.Lose;
            }
        }

        private void CheckForWinCondition()
        {
            EnsureInitialised();

            _instance.WinReasons = new List<WinReasons>();
            if (!_instance.Stats.WinConditionMet()) return;
            _instance.WinReasons.Add(WinReasons.AchievedFameTarget);
            _instance.State = GameState.Win;
        }
    }
}