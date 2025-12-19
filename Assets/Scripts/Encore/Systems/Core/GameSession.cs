using System;
using System.Collections.Generic;
using Encore.Model.Game;

namespace Encore.Systems.Core
{
    [Serializable]
    public sealed class GameSession
    {
        public PlayState PlayState { get; set; } = PlayState.Playing;
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;

        public List<LoseReasons> LoseReasons { get; set; } = new();
        public List<WinReasons> WinReasons { get; set; } = new();
    }
}
