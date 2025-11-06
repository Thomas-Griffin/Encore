using Encore.Abstractions.Interfaces;

namespace Encore.Model.Game
{
    public abstract class GameAction : ISelectable
    {
        public readonly GameActions ActionType;

        protected GameAction(GameActions actionType)
        {
            ActionType = actionType;
        }

        public void OnSelect()
        {
        }
    }
}