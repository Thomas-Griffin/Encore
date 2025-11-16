using Encore.Abstractions.Interfaces;
using UnityEngine;

namespace Encore.Model.Stats
{
    public class GameStat : IVariableStat
    {
        public GameStats Stat { get; }

        public int CurrentValue { get; set; }

        public int LastValue { get; set; }

        // Floating display value for smooth UI animation.
        public float DisplayValue { get; set; }

        public int InitialValue;

        public int MinValue { get; }
        public int MaxValue { get; }

        // colour associated with this stat for UI purposes
        public Color Colour { get; }

        // whether a bonus is currently active for this stat
        private readonly bool _bonusActive;

        // whether random stat changes are being used
        private readonly bool _useRandomStatChanges;

        // bounding numbers for stat increases/decreases after bonuses are considered
        public readonly int MinimumIncrease;
        public readonly int MaximumIncrease;
        public readonly int MinimumDecrease;
        public readonly int MaximumDecrease;

        public GameStat(GameStats stat, Color colour, int initialValue = 0, int minValue = 0, int maxValue = 100,
            int maximumIncrease = 20, int maximumDecrease = 20, int minimumIncrease = 1, int minimumDecrease = 1)
        {
            Stat = stat;
            Colour = colour;
            InitialValue = initialValue;
            LastValue = initialValue;
            DisplayValue = initialValue;
            CurrentValue = initialValue;
            MinValue = minValue;
            MaxValue = maxValue;
            _bonusActive = false;
            _useRandomStatChanges = false;
            MinimumIncrease = minimumIncrease;
            MaximumIncrease = maximumIncrease;
            MinimumDecrease = minimumDecrease;
            MaximumDecrease = maximumDecrease;
        }

        public void Increase()
        {
            IncreaseBy(CalculateIncreaseAmount());
        }

        public void IncreaseBy(int amount)
        {
            LastValue = CurrentValue;
            DisplayValue = CurrentValue;
            CurrentValue += amount;
            ClampValue();
        }

        private int CalculateIncreaseAmount()
        {
            // If random stat changes are enabled, return a random value within the defined range
            if (_useRandomStatChanges)
            {
                return Random.Range(MinimumIncrease, MaximumIncrease + 1); // +1 because upper bound is exclusive
            }

            // Otherwise, return fixed increase amount based on bonus status
            return _bonusActive ? MaximumIncrease : MinimumIncrease;
        }

        public void Decrease()
        {
            LastValue = CurrentValue;
            DisplayValue = CurrentValue;
            CurrentValue -= CalculateDecreaseAmount();
            ClampValue();
        }

        public void DecreaseBy(int amount)
        {
            LastValue = CurrentValue;
            DisplayValue = CurrentValue;
            CurrentValue -= amount;
            ClampValue();
        }

        public void ClampValue()
        {
            if (CurrentValue > MaxValue)
            {
                LastValue = CurrentValue;
                DisplayValue = CurrentValue;
                CurrentValue = MaxValue;
            }
            else if (CurrentValue < MinValue)
            {
                LastValue = CurrentValue;
                DisplayValue = CurrentValue;
                CurrentValue = MinValue;
            }
        }

        public void Reset()
        {
            LastValue = CurrentValue;
            DisplayValue = CurrentValue;
            CurrentValue = InitialValue;
        }

        private int CalculateDecreaseAmount()
        {
            // If random stat changes are enabled, return a random value within the defined range
            if (_useRandomStatChanges)
            {
                return Random.Range(MinimumDecrease, MaximumDecrease + 1); // +1 because upper bound is exclusive
            }

            // Otherwise, return fixed decrease amount based on bonus status
            return _bonusActive ? MaximumDecrease : MinimumDecrease;
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
}