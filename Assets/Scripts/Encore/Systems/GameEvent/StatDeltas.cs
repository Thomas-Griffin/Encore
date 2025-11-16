using System;

namespace Encore.Systems.GameEvent
{
    [Serializable]
    public class StatDeltas
    {
        public int energyDelta;
        public int skillDelta;
        public int popularityDelta;
        public int fameDelta;
    }
}