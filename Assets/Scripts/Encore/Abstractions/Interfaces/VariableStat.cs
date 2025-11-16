using Encore.Model.Stats;

namespace Encore.Abstractions.Interfaces
{
    public interface IVariableStat
    {
        GameStats Stat { get; }
        int CurrentValue { get; set; }
        int MinValue { get; }
        int MaxValue { get; }
        void Increase();
        void Decrease();
        int ClampedValue(int value);
        void Reset();
        bool IsAtMaximum();
        bool IsAtMinimum();
    }
}