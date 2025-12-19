using Encore.Systems.Core;

namespace Encore.Abstractions.Interfaces
{
    public interface IGameContext
    {
        GameManager GameManager { get; }
        GameSession Session { get; }
        IStatService Stats { get; }
        IEventStore Events { get; }
        IDayService DayService { get; }
        ISaveService SaveService { get; }
    }

}