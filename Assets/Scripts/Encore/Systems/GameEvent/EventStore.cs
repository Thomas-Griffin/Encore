using System.Collections.Generic;
using System.IO;
using Encore.Model.Game;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;
using UnityEngine;

namespace Encore.Systems.GameEvent
{
    public class EventStore
    {
        public List<GameEventBase> Events { get; }
        public string SaveFileName { get; }
        public string SaveFileFullPath { get; }

        public EventStore(string saveName)
        {
            SaveFileName = saveName;
            string saveDir = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
            SaveFileFullPath = Path.Combine(saveDir, saveName + ".jsonl");
            Events = new List<GameEventBase>();
            foreach (GameEventBase gameEvent in LoadAll())
            {
                Events.Add(gameEvent);
            }
        }

        public void Append(GameEventBase gameEvent)
        {
            Events.Add(gameEvent);
            EventSnapshot snap = EventSnapshot.FromGameEvent(gameEvent);
            File.AppendAllText(SaveFileFullPath, JsonUtility.ToJson(snap) + "\n");
        }

        private IEnumerable<GameEventBase> LoadAll()
        {
            if (!File.Exists(SaveFileFullPath)) yield break;
            foreach (string line in File.ReadLines(SaveFileFullPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                EventSnapshot snap = JsonUtility.FromJson<EventSnapshot>(line);
                if (snap == null || string.IsNullOrWhiteSpace(snap.eventType)) continue;
                GameEventBase ev = EventSnapshot.ToGameEvent(snap);
                if (ev != null) yield return ev;
            }
        }


        public List<GameEventBase> GetAllEvents() => Events;

        public GameEventBase GetLastEvent() => Events.Count > 0 ? Events[^1] : null;

        public GameAction GetLastAction()
        {
            for (int i = Events.Count - 1; i >= 0; i--)
            {
                if (Events[i].Action != null)
                {
                    return Events[i].Action;
                }
            }

            return null;
        }
    }
}