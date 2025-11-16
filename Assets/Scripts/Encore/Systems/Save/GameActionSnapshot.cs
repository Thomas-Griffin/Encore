using System;
using Encore.Model.Game;

namespace Encore.Systems.Save
{
    [Serializable]
    public class GameActionSnapshot
    {
        public string actionType;


        public static GameActionSnapshot FromGameAction(GameAction action)
        {
            if (action == null) return null;
            return new GameActionSnapshot
            {
                actionType = action.Type.ToString()
            };
        }


        public static GameAction ToGameAction(GameActionSnapshot snapshot)
        {
            if (snapshot == null) return null;
            return Enum.TryParse(snapshot.actionType, out GameActions actionType) ? new SimpleGameAction(actionType) : null;
        }
    }
}