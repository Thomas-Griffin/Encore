using Encore.Systems.GameEvent.Events;

namespace Encore.Model.Game
{
    public class SimpleGameAction : GameAction
    {
        public SimpleGameAction(GameActions type) : base(type)
        {
        }

        public override GameEventBase ToGameEvent()
        {
            switch (Type)
            {
                case GameActions.Rest:
                {
                    RestEvent restEvent = new()
                    {
                        Action = this
                    };
                    return restEvent;
                }
                case GameActions.Practice:
                {
                    PractiseEvent practiseEvent = new()
                    {
                        Action = this
                    };
                    return practiseEvent;
                }
                case GameActions.Gig:
                {
                    GigEvent gigEvent = new()
                    {
                        Action = this
                    };
                    return gigEvent;
                }
            }

            return null;
        }
    }
}