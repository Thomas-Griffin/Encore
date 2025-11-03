using System.Collections.Generic;

public class StatManager
{
    // Main stats
    public readonly GameStat Energy = new(GameStats.Energy);
    public readonly GameStat Skill = new(GameStats.Skill);
    public readonly GameStat Popularity = new(GameStats.Popularity);
    public readonly GameStat Fame = new(GameStats.Fame);

    public void InitialiseStats(DifficultyLevel difficulty)
    {
        int initialEnergy = difficulty switch
        {
            DifficultyLevel.Easy => 100,
            DifficultyLevel.Medium => 80,
            DifficultyLevel.Hard => 60,
            _ => 100
        };
        Energy.InitialValue = initialEnergy;
        Energy.CurrentValue = initialEnergy;
        Skill.CurrentValue = 0;
        Popularity.CurrentValue = 0;
        Fame.CurrentValue = 0;
    }

    public void IncreaseStat(GameStats stat)
    {
        GetStat(stat).Increase();
    }

    public void DecreaseStat(GameStats stat)
    {
        GetStat(stat).Decrease();
    }

    private GameStat GetStat(GameStats stat)
    {
        return stat switch
        {
            GameStats.Energy => Energy,
            GameStats.Skill => Skill,
            GameStats.Popularity => Popularity,
            GameStats.Fame => Fame,
            _ => throw new KeyNotFoundException($"Stat {stat} not found.")
        };
    }

    public bool LoseConditionMet()
    {
        return Energy.IsAtMinimum();
    }

    public bool WinConditionMet()
    {
        return Fame.IsAtMaximum();
    }

    public void ResetStats()
    {
        foreach (var stat in GetAllStats())
        {
            stat.Reset();
        }
    }

    private IEnumerable<GameStat> GetAllStats()
    {
        yield return Energy;
        yield return Skill;
        yield return Popularity;
        yield return Fame;
    }

    // Calculate and add fame bonus based on skill and popularity
    public void AddFameBonus()
    {
        int skillFameWorth = 10; // 1 fame for every skillFameWorth points
        int popularityFameWorth = 15; // 1 fame for every popularityFameWorth points
        int skillBonus = Skill.CurrentValue / skillFameWorth;
        int popularityBonus = Popularity.CurrentValue / popularityFameWorth;
        int fameBonus = skillBonus + popularityBonus;
        Fame.IncreaseBy(fameBonus * FameScaleFactor());
    }

    // Determine fame scaling factor based on difficulty
    private static int FameScaleFactor()
    {
        return GameManager.CurrentDifficulty switch
        {
            DifficultyLevel.Easy => 1,
            DifficultyLevel.Medium => 2,
            DifficultyLevel.Hard => 3,
            _ => 1
        };
    }
}