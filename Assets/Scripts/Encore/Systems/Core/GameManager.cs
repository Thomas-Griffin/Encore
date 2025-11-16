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

        public GameInstance Instance { get; private set; }

        private bool _initialised;

        public GameManager()
        {
        }

        public GameManager(GameInstance instance)
        {
            Instance = instance;
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

            Instance ??= new GameInstance();

            if (!Instance.Stats)
            {
                Instance.Stats = ScriptableObject.CreateInstance<StatManager>();
            }

            Instance.Days ??= new DayManager(Instance.Difficulty);

            _initialised = true;
        }

        public void StartGame(DifficultyLevel difficulty)
        {
            EnsureInitialised();

            if (SaveManager.SaveFileExists(Instance?.SaveFileName))
            {
                Instance = SaveManager.LoadFromFile(Instance?.SaveFileName).ToGameInstance();
                return;
            }

            Instance ??= new GameInstance(DateTime.UtcNow.ToString("F"), difficulty);

            Instance.Difficulty = difficulty;
            Instance.Stats.InitialiseStats(difficulty);
            Instance.State = GameState.Playing;
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

                Instance.Events.Append(Instance.ApplyEvent(gameEvent));
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

            string saveName = Instance.SaveFileName;
            if (string.IsNullOrWhiteSpace(saveName))
            {
                saveName = "autosave";
            }

            SaveManager.SaveToFile(new SaveData(saveName, 0, Instance, DateTime.UtcNow));
        }

        public void CheckForEndGameConditions()
        {
            EnsureInitialised();
            CheckForWinCondition();
            CheckForLoseCondition();
            switch (Instance?.State)
            {
                case GameState.Win:
                    UIScreenManager.Instance.ShowScreen(UIScreenNames.WinScreen);
                    break;
                case GameState.Lose:
                    UIScreenManager.Instance.ShowScreen(UIScreenNames.LoseScreen);
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
            Instance.LoseReasons = new List<LoseReasons>();
            if (Instance.Stats.LoseConditionMet())
            {
                Instance.LoseReasons.Add(LoseReasons.EnergyDepleted);
                Instance.State = GameState.Lose;
            }
            else if (Instance.Days.CurrentDay == Instance.Days.TotalDays)
            {
                if (Instance.Stats.WinConditionMet()) return;
                Instance.LoseReasons.Add(LoseReasons.RanOutOfTime);
                Instance.State = GameState.Lose;
            }
        }

        private void CheckForWinCondition()
        {
            EnsureInitialised();

            Instance.WinReasons = new List<WinReasons>();
            if (!Instance.Stats.WinConditionMet()) return;
            Instance.WinReasons.Add(WinReasons.AchievedFameTarget);
            Instance.State = GameState.Win;
        }
    }
}