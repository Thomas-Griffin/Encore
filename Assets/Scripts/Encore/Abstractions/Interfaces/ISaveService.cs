using System.Collections.Generic;
using Encore.Systems.Save;

namespace Encore.Abstractions.Interfaces
{
    public interface ISaveService
    {
        void Save(SavedGame savedGame, IEnumerable<EventSnapshot> events);
        SavedGame Load();
        void DeleteSave();
    }
}