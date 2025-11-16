using Encore.Model.Game;

namespace Encore.Systems.Core
{
    public class DayManager
    {
        public int CurrentDay { get; private set; } = 1;
        public int TotalDays { get; private set; }


        public DayManager(DifficultyLevel difficultyLevel)
        {
            TotalDays = difficultyLevel switch
            {
                DifficultyLevel.Easy => 50,
                DifficultyLevel.Medium => 45,
                DifficultyLevel.Hard => 40,
                _ => 50
            };
        }

        public void Advance()
        {
            if (CurrentDay < TotalDays) CurrentDay++;
        }

        public void Reset()
        {
            CurrentDay = 1;
        }
    }
}