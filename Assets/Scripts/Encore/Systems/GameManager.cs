using Encore.Model.Game;
using Encore.Model.Stats;
using UnityEngine;

namespace Encore.Systems
{
    public class GameManager : MonoBehaviour
    {
        public StatManager statManager;

        public GameState currentGameState = GameState.Menu;

        public GameActions lastAction;

        public static DifficultyLevel CurrentDifficulty;

        public int currentDay = 1;
        public int totalDays = 40;

        public int skillBarrierForGig = 3;

        void Awake()
        {
            // Ensure a StatManager component exists in the scene and assign it
            if (!statManager)
            {
                statManager = FindAnyObjectByType<StatManager>();
                if (!statManager)
                {
                    var statManagerContainer = new GameObject("StatManager");
                    statManager = statManagerContainer.AddComponent<StatManager>();
                    DontDestroyOnLoad(statManagerContainer);
                }
            }
        }

        public void StartGame(DifficultyLevel difficulty)
        {
            CurrentDifficulty = difficulty;
            statManager.InitialiseStats(difficulty);
            currentGameState = GameState.Playing;
        }

        // Resting increases energy but costs popularity
        public void DoRest()
        {
            statManager.IncreaseStat(GameStats.Energy);
            statManager.DecreaseStat(GameStats.Popularity);
            statManager.DecreaseStat(GameStats.Skill);
        }

        // Practicing increases skill but costs energy
        public void DoPractice()
        {
            statManager.IncreaseStat(GameStats.Skill);
            statManager.DecreaseStat(GameStats.Energy);
            if (lastAction == GameActions.Practice)
            {
                Debug.Log("You practiced two days in a row! Skill gain is increased, but you spend more energy.");
                statManager.IncreaseStat(GameStats.Skill);
                statManager.DecreaseStat(GameStats.Energy);
            }
        }

        // A gig increases popularity and fame but costs energy
        public void DoGig()
        {
            if (statManager.Skill.CurrentValue < skillBarrierForGig)
            {
                Debug.Log("Not skilled enough to perform a gig! Practice more first.");
                return;
            }

            statManager.IncreaseStat(GameStats.Popularity);
            statManager.IncreaseStat(GameStats.Fame);
            statManager.DecreaseStat(GameStats.Energy);
        }

        public void DoAction(GameActions action)
        {
            switch (action)
            {
                case GameActions.Rest:
                    DoRest();
                    break;
                case GameActions.Practice:
                    DoPractice();
                    break;
                case GameActions.Gig:
                    DoGig();
                    break;
                default:
                    Debug.LogWarning($"Unhandled action: {action.ToString()}");
                    break;
            }

            lastAction = action;
            currentDay += 1;
            CheckForEndGameConditions();
        }

        public void CheckForEndGameConditions()
        {
            CheckForWinCondition();
            CheckForLoseCondition();
            if (currentGameState == GameState.Win)
            {
                Debug.Log("You Win!");
            }
            else if (currentGameState == GameState.Lose)
            {
                Debug.Log("You Lose!");
            }
        }


        private void CheckForLoseCondition()
        {
            if (statManager.LoseConditionMet() || (currentDay == totalDays && !statManager.WinConditionMet()))
            {
                currentGameState = GameState.Lose;
            }
        }

        void CheckForWinCondition()
        {
            if (statManager.WinConditionMet())
            {
                currentGameState = GameState.Win;
            }
        }
    }
}