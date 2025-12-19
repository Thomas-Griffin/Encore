using Encore.Model.Game;
using UnityEngine;

namespace Encore.Systems.Configurations
{
    [CreateAssetMenu(menuName = "Encore/Stats Config")]
    public sealed class StatsConfig : ScriptableObject
    {
        [Header("Targets")] public int fameTargetEasy = 100;
        public int fameTargetMedium = 200;
        public int fameTargetHard = 300;

        [Header("Colours")] public Color energyColor = Color.green;
        public Color skillColor = Color.orange;
        public Color popularityColor = Color.blue;
        public Color fameColor = Color.purple;

        public int GetFameTarget(Difficulty difficulty) => difficulty switch
        {
            Difficulty.Easy => fameTargetEasy,
            Difficulty.Medium => fameTargetMedium,
            Difficulty.Hard => fameTargetHard,
            _ => fameTargetEasy
        };
    }
}