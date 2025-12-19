using System.Collections.Generic;
using Encore.Abstractions.Interfaces;

namespace Encore.Systems.Save
{
    public sealed class InMemorySaveService : ISaveService
    {
        private SavedGame _saved;
        private List<EventSnapshot> _events;

        public void Save(SavedGame savedGame, IEnumerable<EventSnapshot> events)
        {
            _saved = savedGame;
            _events = events != null ? new List<EventSnapshot>(events) : new List<EventSnapshot>();
            if (_saved?.saveData != null)
            {
                _saved.saveData.events = new List<EventSnapshot>(_events);
            }
        }

        public SavedGame Load()
        {
            return _saved;
        }

        public void DeleteSave()
        {
            _saved = null;
            _events = null;
        }
    }
}