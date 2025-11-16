using Encore.Abstractions.Interfaces;
using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Game
{
    public abstract class GameAction : ISelectable
    {
        public readonly GameActions Type;

        protected GameAction(GameActions type)
        {
            Type = type;
        }
        
        public void OnSelect()
        {
        }
        
        public abstract GameEventBase ToGameEvent();
    }
}