using System;
using Encore.Model.Game;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;
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

            if (Instance is { State: GameState.Win or GameState.Lose })
            {
                Debug.Log("Previous game ended. Starting a new game.");
            }

            if (SaveManager.SaveFileExists(Instance?.SaveFileName))
            {
                Debug.Log("Existing save file found. Loading previous game state.");
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

        public void RecordAndApplyEvent(GameEventBase gameEvent)
        {
            EnsureInitialised();

            Instance.Events.Append(Instance.ApplyEvent(gameEvent));
            if (gameEvent.DelegateEvent != null)
            {
                RecordAndApplyEvent(gameEvent.DelegateEvent);
            }
        }

        public void PerformAutoSave()
        {
            EnsureInitialised();

            string saveName = Instance.SaveFileName;
            if (string.IsNullOrWhiteSpace(saveName))
            {
                saveName = "autosave";
            }

            SaveManager.SaveToFile(new SaveData(saveName, 0, Instance, DateTime.UtcNow));
            Debug.Log("Game auto-saved.");
        }

        public void CheckForEndGameConditions()
        {
            EnsureInitialised();
            CheckForWinCondition();
            CheckForLoseCondition();
            switch (Instance?.State)
            {
                case GameState.Win:
                    Debug.Log("You Win!");
                    break;
                case GameState.Lose:
                    Debug.Log("You Lose!");
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

            if (Instance.Stats.LoseConditionMet() || (Instance.Days.CurrentDay == Instance.Days.TotalDays &&
                                                      !Instance.Stats.WinConditionMet()))
            {
                Instance.State = GameState.Lose;
            }
        }

        private void CheckForWinCondition()
        {
            EnsureInitialised();

            if (Instance.Stats.WinConditionMet())
            {
                Instance.State = GameState.Win;
            }
        }
    }
}