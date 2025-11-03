using UnityEngine;

public class GameManager
{
    public readonly StatManager StatManager = new();

    public GameState CurrentGameState = GameState.Menu;
    
    public static DifficultyLevel CurrentDifficulty;

    public void StartGame(DifficultyLevel difficulty)
    {
        CurrentDifficulty = difficulty;
        StatManager.InitialiseStats(difficulty);
        CurrentGameState = GameState.Playing;
    }
    
    // Resting increases energy but costs popularity
    public void DoRest()
    {
        StatManager.IncreaseStat(GameStats.Energy);
        StatManager.DecreaseStat(GameStats.Popularity);
    }

    // Practicing increases skill but costs energy
    public void DoPractice()
    {
        StatManager.IncreaseStat(GameStats.Skill);
        StatManager.DecreaseStat(GameStats.Energy);
    }

    // A gig increases popularity and fame but costs energy
    public void DoGig()
    {
        StatManager.IncreaseStat(GameStats.Popularity);
        StatManager.IncreaseStat(GameStats.Fame);
        StatManager.DecreaseStat(GameStats.Energy);
    }

    public void DoAction(GameAction action)
    {
        switch (action.ActionType)
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
                Debug.LogWarning($"Unhandled action: {action}");
                break;
        }

        CheckForEndGameConditions();
    }

    public void CheckForEndGameConditions()
    {
        CheckForWinCondition();
        CheckForLoseCondition();
        if (CurrentGameState == GameState.Win)
        {
            Debug.Log("You Win!");
        }
        else if (CurrentGameState == GameState.Lose)
        {
            Debug.Log("You Lose!");
        }
    }


    private void CheckForLoseCondition()
    {
        if (StatManager.LoseConditionMet())
        {
            CurrentGameState = GameState.Lose;
        }
    }

    void CheckForWinCondition()
    {
        if (StatManager.WinConditionMet())
        {
            CurrentGameState = GameState.Win;
        }
    }
}