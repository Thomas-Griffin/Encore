using Encore.Abstractions.Interfaces;
using Encore.Model.Game;

namespace Encore.Systems.Core
{
    public sealed class DayService : IDayService
    {
        public int CurrentDay { get; private set; }
        public int TotalDays { get; private set; }

        public void Initialise(Difficulty difficulty)
        {
            TotalDays = difficulty switch
            {
                Difficulty.Easy => 50,
                Difficulty.Medium => 45,
                Difficulty.Hard => 40,
                _ => 50
            };

            CurrentDay = 1;
        }

        public void Advance()
        {
            if (CurrentDay < TotalDays)
                CurrentDay++;
        }

        public void Reset()
        {
            CurrentDay = 1;
        }
    }
}