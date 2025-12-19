using System;
using Encore.Model.Player;
using Encore.Model.Player.Actions;

namespace Encore.Systems.Save
{
    [Serializable]
    public class GameActionSnapshot
    {
        public string actionType;


        public static GameActionSnapshot FromGameAction(PlayerAction action)
        {
            if (action == null) return null;
            return new GameActionSnapshot
            {
                actionType = action.GetType().Name,
            };
        }


        public static PlayerAction ToGameAction(GameActionSnapshot snapshot)
        {
            if (snapshot == null) return null;
            return snapshot.actionType switch
            {
                "Practice" => new Practice(),
                "Rest" => new Rest(),
                "Gig" => new Gig(),
                _ => null
            };
        }
    }
}