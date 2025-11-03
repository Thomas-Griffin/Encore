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