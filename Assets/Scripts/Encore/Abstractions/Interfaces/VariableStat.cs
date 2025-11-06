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
        void ClampValue();
        void Reset();
        bool IsAtMaximum();
        bool IsAtMinimum();
    }
}