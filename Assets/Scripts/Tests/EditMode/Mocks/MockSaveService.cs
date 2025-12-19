using System.Collections.Generic;
using Encore.Abstractions.Interfaces;
using Encore.Systems.Save;

namespace Tests.EditMode.Mocks
{
    public sealed class MockSaveService : ISaveService
    {
        public int SaveCallCount { get; private set; }

        public void Save(SavedGame savedGame, IEnumerable<EventSnapshot> events) =>
            SaveCallCount++;


        public SavedGame Load() => null;

        public void DeleteSave()
        {
        }
    }
}