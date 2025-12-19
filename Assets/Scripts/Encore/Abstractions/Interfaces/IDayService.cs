using Encore.Model.Game;

namespace Encore.Abstractions.Interfaces
{
    public interface IDayService
    {
        int CurrentDay { get; }
        int TotalDays { get; }
        void Initialise(Difficulty difficulty);
        void Advance();
        void Reset();
    }
}