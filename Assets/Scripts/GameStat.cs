using UnityEngine;

public class GameStat : IVariableStat
{
    // the name of the stat
    public GameStats Stat { get; }

    // Current value of the stat
    public int CurrentValue { get; set; }
    
    // Initial value of the stat
    public int InitialValue;

    // Maximum and minimum bounds for the stat
    public int MinValue { get; }
    public int MaxValue { get; }

    // whether a bonus is currently active for this stat
    private readonly bool _bonusActive;

    // whether random stat changes are being used
    private readonly bool _useRandomStatChanges;

    // bounding numbers for stat increases/decreases after bonuses are considered
    private readonly int _minimumIncrease;
    private readonly int _maximumIncrease;
    private readonly int _minimumDecrease;
    private readonly int _maximumDecrease;

    public GameStat(GameStats stat, int initialValue = 0, int minValue = 0, int maxValue = 100)
    {
        Stat = stat;
        InitialValue = initialValue;
        CurrentValue = initialValue;
        MinValue = minValue;
        MaxValue = maxValue;
        _bonusActive = false;
        _useRandomStatChanges = false;
        _minimumIncrease = 1;
        _maximumIncrease = 5;
        _minimumDecrease = 1;
        _maximumDecrease = 5;
    }

    public void Increase()
    {
        IncreaseBy(CalculateIncreaseAmount());
    }

    public void IncreaseBy(int amount)
    {
        CurrentValue += amount;
        ClampValue();
    }

    private int CalculateIncreaseAmount()
    {
        // If random stat changes are enabled, return a random value within the defined range
        if (_useRandomStatChanges)
        {
            return Random.Range(_minimumIncrease, _maximumIncrease + 1); // +1 because upper bound is exclusive
        }

        // Otherwise, return fixed increase amount based on bonus status
        return _bonusActive ? _maximumIncrease : _minimumIncrease;
    }

    public void Decrease()
    {
        CurrentValue -= CalculateDecreaseAmount();
        ClampValue();
    }

    public void ClampValue()
    {
        if (CurrentValue > MaxValue)
        {
            CurrentValue = MaxValue;
        }
        else if (CurrentValue < MinValue)
        {
            CurrentValue = MinValue;
        }
    }

    public void Reset()
    {
        CurrentValue = InitialValue;
    }

    private int CalculateDecreaseAmount()
    {
        // If random stat changes are enabled, return a random value within the defined range
        if (_useRandomStatChanges)
        {
            return Random.Range(_minimumDecrease, _maximumDecrease + 1); // +1 because upper bound is exclusive
        }

        // Otherwise, return fixed decrease amount based on bonus status
        return _bonusActive ? _maximumDecrease : _minimumDecrease;
    }
    
    public bool IsAtMaximum()
    {
        return CurrentValue == MaxValue;
    }

    public bool IsAtMinimum()
    {
        return CurrentValue == MinValue;
    }

    public bool HasExceededMaximum()
    {
        return CurrentValue > MaxValue;
    }

    public bool HasExceededMinimum()
    {
        return CurrentValue < MinValue;
    }
}